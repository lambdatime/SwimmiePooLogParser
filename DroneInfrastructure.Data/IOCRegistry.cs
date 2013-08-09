using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCore;
using SwimmiePooLogParserDrone.Core.Repositories;
using SwimmiePooLogParserDrone.Infrastructure.Data.Repositories;

namespace SwimmiePooLogParserDrone.Infrastructure.Data
{
    public class IOCRegistry : IIocRegistry
    {
        public void Register(dynamic kernel)
        {
            kernel.Bind<IRequestRepository>().To<RequestRepository>();
        }
    }
}
