using SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>
    {
        public UserProfileRepository(SecurityContext context) : base(context) { }
    }
}