using System.Data.Entity;
using SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories
{
    public class SecurityContext : DbContext
    {
        public SecurityContext()
            : base("MembershipConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ActiveRole> ActiveRoles { get; set; }
    }
}
