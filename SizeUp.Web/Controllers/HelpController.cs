using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;

namespace SizeUp.Web.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /Help/
        [ActionName("How-It-Works")]
        public ActionResult HowItWorks()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "HowItWorks.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View("HowItWorks");
        }

        public ActionResult Faq()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Faq.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Contact.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult EnterprisePartners()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "EnterprisePartners.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult EnterprisePartnersDownload()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "EnterprisePartners.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

    }
}
