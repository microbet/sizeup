using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Controllers
{
    public class FinTinderController : Controller
    {
        //
        // GET: /FinTinder/

        public ActionResult Index()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            return View();
        }

    }
}
