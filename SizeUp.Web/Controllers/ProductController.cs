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
            ViewBag.HideLogin = Request.QueryString["hideLogin"];

            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };

            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Terms.Content").Select(i => i.Value).FirstOrDefault();
                //hot fix
                if (ViewBag.HideLogin == "1")
                {
                    string content = ViewBag.Content;
                    ViewBag.Content = content.Replace("product/privacy-policy", "product/privacy-policy?hideLogin=1");
                }
            }
            return View();
        }

        [ActionName("Privacy-Policy")]
        public ActionResult Privacy()
        {
            ViewBag.HideLogin = Request.QueryString["hideLogin"];
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Privacy.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View("Privacy");
        }

    }
}
