using System;
using System.Collections.Generic;

namespace CommonCore
{
    public interface IIocDependencyResolver
    {
        T Get<T>();
        IEnumerable<T> GetAll<T>();
        object Get(Type type);
        IEnumerable<object> GetAll(Type type);
    }
}