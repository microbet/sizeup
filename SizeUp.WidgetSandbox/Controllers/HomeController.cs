using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.WidgetSandbox.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/ 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HighContrast()
        {
            return View();
        }

    }
}
