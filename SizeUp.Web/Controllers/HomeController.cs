using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Serialization;
using Api = SizeUp.Web.Areas.Api;

namespace SizeUp.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            return View("IndexResponsive");
        }
    }
}
