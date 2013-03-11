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

        public ActionResult Wide()
        {
            return View();
        }

        public ActionResult Prod()
        {
            return View();
        }

        public ActionResult SBA()
        {
            return View();
        }

        public ActionResult Inc()
        {
            return View();
        }

        public ActionResult BestPlaces()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Embed()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Embed(string embedCode)
        {
            ViewBag.Code = embedCode;
            return View();
        }




    }
}
