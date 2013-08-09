using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SwimmiePooLogParser.Common.DTOs;
using SwimmiePooLogParserDrone.UI.Helpers.ModelBinders;
using WebApiContrib.Formatting.Jsonp;

namespace SwimmiePooLogParserDrone.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ModelBinders.Binders.Add(typeof(WorkLoad), new WorkLoadBinder());

        }
    }
}