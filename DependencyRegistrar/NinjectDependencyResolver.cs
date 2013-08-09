using System;
using System.Collections.Generic;
using CommonCore;
using Ninject;

namespace SwimmiePooLogParser.DependencyRegistrar
{
    public class NinjectDependencyResolver : IIocDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _kernel.GetAll<T>();
        }

        public object Get(Type type)
        {
            return _kernel.Get(type);
        }

        public IEnumerable<object> GetAll(Type type)
        {
            return _kernel.GetAll(type);
        }
    }
}