namespace CommonCore
{
    public interface IIocDependencyRegistrerer
    {
        void RegisterAllOfInterface<T1>(string assemblyPattern);
        void RegisterAllOfInterfaceInPath<T1>(string path);
    }
}