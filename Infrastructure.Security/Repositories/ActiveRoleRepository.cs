using SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories
{
    public class ActiveRoleRepository : GenericRepository<ActiveRole>
    {
        public ActiveRoleRepository(SecurityContext context) : base(context) { }
    }
}