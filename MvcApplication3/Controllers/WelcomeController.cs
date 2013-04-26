using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication3.Controllers
{
    public class WelcomeController : Controller
    {
        //
        // GET: /Welcome/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Welcome(String name, int numTimes)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTime = numTimes;
            return View();
        }
    }
}
