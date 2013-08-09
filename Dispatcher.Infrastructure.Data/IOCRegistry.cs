using CommonCore;
using Dispatcher.Infrastructure.Data;
using SwimmiePooLogParserDispatcher.Core.Repositories;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Data
{
    public class IOCRegistry : IIocRegistry
    {
        public void Register(dynamic kernel)
        {
            kernel.Bind<IDroneRepository>().To<DroneRepository>();
            kernel.Bind<ICurrentParseLinesRepository>().To<CurrentParseLinesRepository>();
        }
    }
}
