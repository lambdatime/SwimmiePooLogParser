using System.IO;
using System.Linq;
using System.Reflection;
using CommonCore;
using Ninject;
using Ninject.Extensions.Conventions;

namespace SwimmiePooLogParser.DependencyRegistrar
{
    public class NinjectDependencyRegisterer : IIocDependencyRegistrerer
    {
        private readonly IKernel _kernel;

        public NinjectDependencyRegisterer(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void RegisterAllOfInterface<T1>(string assemblyPattern)
        {
            _kernel.Bind((c) =>
                         c.FromAssembliesMatching("*")
                          .Select(t => t.GetInterfaces().Any(i => i == typeof(T1)))
                          .BindAllInterfaces());
        }

        public void RegisterAllOfInterfaceInPath<T1>(string path)
        {
            var dlls = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            var assemblies = dlls.Select(Assembly.LoadFrom);
            _kernel.Bind((c) =>
                         c.From(assemblies)
                          .Select(t => t.GetInterfaces().Any(i => i == typeof(T1)))
                          .BindAllInterfaces());
        }
    }
}