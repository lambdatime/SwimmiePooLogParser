using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwimmiePooLogParserDispatcher.Core;
using SwimmiePooLogParserDispatcher.Core.Repositories;

namespace SwimmiePooLogParserDispatcher.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDroneRepository _droneRepository;

        public HomeController(IDroneRepository droneRepository)
        {
            _droneRepository = droneRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
