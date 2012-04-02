using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;

namespace SizeUp.Web.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        public ActionResult Terms()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            ViewBag.Content = DataContexts.SizeUpContext.ResourceStrings.Where(i => i.Name == "Terms.Content").Select(i => i.Value).FirstOrDefault();
            return View();
        }

        [ActionName("Privacy-Policy")]
        public ActionResult Privacy()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            ViewBag.Content = DataContexts.SizeUpContext.ResourceStrings.Where(i => i.Name == "Privacy.Content").Select(i => i.Value).FirstOrDefault();
            return View("Privacy");
        }

    }
}
