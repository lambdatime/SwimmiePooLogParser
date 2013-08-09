using System;
using System.Collections.Generic;

namespace CommonCore
{
    public static class DependencyResolver
    {
        private static IIocDependencyResolver _dependencyResolver;

        public static void SetResolver(IIocDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public static T Get<T>()
        {
            return _dependencyResolver.Get<T>();
        }
        public static IEnumerable<T> GetAll<T>()
        {
            return _dependencyResolver.GetAll<T>();
        }

        public static object Get(Type type)
        {
            return _dependencyResolver.Get(type);
        }
        public static IEnumerable<object> GetAll(Type type)
        {
            return _dependencyResolver.GetAll(type);
        }
    }
}