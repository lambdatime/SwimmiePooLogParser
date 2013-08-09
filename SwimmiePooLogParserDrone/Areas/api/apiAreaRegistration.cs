using System.Web.Http;
using System.Web.Mvc;
using SwimmiePooLogParserDrone.UI.Helpers.Extensions;

namespace SwimmiePooLogParserDrone.UI.Areas.api
{
    public class apiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapHttpRoute("DefaultActionBased", "api/{controller}/{action}/{id}",
                                        new { id = RouteParameter.Optional });

            context.MapHttpRoute("DefaultApi", "api/{controller}/{id}",
                                        new { id = RouteParameter.Optional });
        }
    }
}
