using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;


namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class CompetitionController : BaseController
    {
        //
        // GET: /Wiget/Competition/

        public ActionResult Index()
        {
            ViewBag.Header.ActiveTab = NavItems.Competition;
            return View();
        }

    }
}
