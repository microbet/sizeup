using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using SizeUp.Core.Web;
namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class SelectController : BaseController
    {
        //
        // GET: /Wiget/Select/

        public ActionResult Index()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            ViewBag.StartFeature = WebContext.Current.StartFeature;
            return View();
        }

    }
}
