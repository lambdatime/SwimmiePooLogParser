using CommonCore;
using SwimmiePooLogParser.Common;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security
{
    public class SecurityRegistry : IIocRegistry
    {
        public void Register(dynamic kernel)
        {
            kernel.Bind<IUserManagementService>().To<UserManagementService>();
        }
    }
}
