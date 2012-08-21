using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /Widget/Help/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transcript()
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            return View();
        }

    }
}
