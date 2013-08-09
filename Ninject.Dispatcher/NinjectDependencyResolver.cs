using System.Web.Http.Dependencies;

namespace Ninject.SwimmiePooLogParserDispatcher
{
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }
}