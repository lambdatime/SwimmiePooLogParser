using System.Web;

namespace SwimmiePooLogParser.DependencyRegistrar
{
    public class NinjectDependencyRegistrarModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
        }

        void context_PreRequestHandlerExecute(object sender, System.EventArgs e)
        {
            NinjectDependencyRegistrar.EnsureDependenciesRegistered();
        }

        public void Dispose() { }
    }
}
