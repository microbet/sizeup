using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;


namespace SizeUp.Web.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Community/

        public ActionResult CommunityWithIndustry()
        {
            ViewBag.Header.ActiveTab = NavItems.Dashboard;
            return View();
        }


        public ActionResult Community()
        {
            ViewBag.Header.ActiveTab = NavItems.Dashboard;
            return View();
        }


        public ActionResult RedirectWithIndustry(string oldSEO, string industry)
        {
            return View();
        }

        public ActionResult Redirect(string oldSEO)
        {
            return View();
        }


    }
}
