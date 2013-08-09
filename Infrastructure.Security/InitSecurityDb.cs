using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories;
using WebMatrix.WebData;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security
{
    public class InitSecurityDb : CreateDatabaseIfNotExists<SecurityContext>
    {
        protected override void Seed(SecurityContext context)
        {

            WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("MembershipConnection",
                                                                       "UserProfile", "UserId", "UserName", autoCreateTables: true);
            
        }

        
    }
}