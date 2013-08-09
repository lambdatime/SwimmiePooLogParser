namespace CommonCore
{
    public static class DependencyRegistrationManager
    {
        private static IIocDependencyRegistrerer _dependencyRegistrerer;

        public static void SetRegisterer(IIocDependencyRegistrerer dependencyRegistrerer)
        {
            _dependencyRegistrerer = dependencyRegistrerer;
        }

        public static void RegisterAllOfInterface<T1>(string assemblyPattern)
        {
            _dependencyRegistrerer.RegisterAllOfInterface<T1>(assemblyPattern);
        }

        public static void RegisterAllOfInterfaceInPath<T1>(string path)
        {
            _dependencyRegistrerer.RegisterAllOfInterfaceInPath<T1>(path);
        }
    }
}
