using System.Collections.Generic;
using System.Linq;
using CommonCore;
using Ninject;
using Ninject.Activation.Strategies;
using Ninject.Extensions.Conventions;

namespace SwimmiePooLogParser.DependencyRegistrar
{
    public class NinjectDependencyRegistrar
    {
        public static List<string> registeredDependencies { get; set; }
        private static bool _dependenciesRegistered;
        public static object _lockable = new object();

        public static void EnsureDependenciesRegistered()
        {
            lock (_lockable)
            {
                if (!_dependenciesRegistered)
                {
                    registeredDependencies = new List<string>();
                    RegisterDependencies();
                    _dependenciesRegistered = true;
                }
            }
        }

        private static void RegisterDependencies()
        {
            IKernel kernel = new StandardKernel();
            kernel.Components.Add<IActivationStrategy, MyMonitorActivationStrategy>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            DependencyRegistrationManager.SetRegisterer(new NinjectDependencyRegisterer(kernel));

            kernel.Bind((c) =>
                        c.FromAssembliesMatching("*")
                         .Select(t => t.GetInterfaces().Any(i => i == typeof(IIocRegistry)))
                         .BindAllInterfaces());

            var registries = kernel.GetAll<IIocRegistry>().ToList();
            foreach (var registry in registries)
            {
                registry.Register(kernel);
            }
        }
    }

    public class MyMonitorActivationStrategy : ActivationStrategy
    {
        public override void Activate(Ninject.Activation.IContext context, Ninject.Activation.InstanceReference reference)
        {
            //if (reference.Instance is ILogger)
            //{
            //    _logger = (ILogger)reference.Instance;
            //}
            //_logger.Debug("Ninject Activate: " + reference.Instance.GetType());
            lock (NinjectDependencyRegistrar._lockable)
            {
                NinjectDependencyRegistrar.registeredDependencies.Add("Ninject Activate: " + reference.Instance.GetType());
            }
            base.Activate(context, reference);
        }

        public override void Deactivate(Ninject.Activation.IContext context, Ninject.Activation.InstanceReference reference)
        {
            NinjectDependencyRegistrar.registeredDependencies.Add("Ninject DeActivate: " + reference.Instance.GetType());
           // _logger.Debug("Ninject DeActivate: " + reference.Instance.GetType());
            base.Deactivate(context, reference);
        }
    }
}