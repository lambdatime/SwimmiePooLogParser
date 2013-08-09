using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using CommonCore;
using SwimmiePooLogParser.Common;

namespace Ninject.SwimmiePooLogParserDispatcher
{
    public class ZApiRegistry : IIocRegistry
    {
        public void Register(dynamic kernel)
        {
            var container = (IKernel)kernel;
            container.Bind<IDependencyResolver>().To<NinjectDependencyResolver>();

            System.Web.Mvc.DependencyResolver.SetResolver(ResolveDependency, ResolveDependencies);
            //GlobalConfiguration.Configuration.DependencyResolver = container.Get<IDependencyResolver>();

            var userMgmtSvc = container.Get<IUserManagementService>();
            userMgmtSvc.Register();
        }

        private static object ResolveDependency(Type type)
        {
            var method = typeof(DependencyResolver)
                .GetMethods()
                .Single(m => m.Name == "Get" && m.IsStatic && m.IsGenericMethod && m.ContainsGenericParameters &&
                             m.ReturnType.IsGenericParameter && !m.GetParameters().Any());
            var generic = method.MakeGenericMethod(type);
            return generic.Invoke(null, new object[] { });
        }

        private static IEnumerable<object> ResolveDependencies(Type type)
        {
            var method = typeof(DependencyResolver)
                .GetMethods()
                .Single(m => m.Name == "GetAll" && m.IsStatic && m.IsGenericMethod && m.ContainsGenericParameters &&
                             !m.GetParameters().Any());
            var generic = method.MakeGenericMethod(type);
            return (IEnumerable<object>)generic.Invoke(null, new object[] { });
        }
    }
}