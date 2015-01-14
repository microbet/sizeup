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
            List<string> p = new List<string>();
            foreach (var x in Request.Form.AllKeys)
            {
                if (!string.IsNullOrEmpty(Request.Form[x]))
                {
                    p.Add(x + "=" + Request.Form[x]);
                }
            }


            ViewBag.Params = String.Join("&", p);
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

        public ActionResult BusinessUSA()
        {
            return View();
        }

        public ActionResult Inc()
        {
            return View();
        }

        public ActionResult Staples()
        {
            return View();
        }

        public ActionResult BestPlaces()
        {
            return View();
        }

        public ActionResult WellsFargo()
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



        public ActionResult API()
        {
            return View();
        }

        public ActionResult APIDocGen()
        {
            return View();
        }



    }
}
