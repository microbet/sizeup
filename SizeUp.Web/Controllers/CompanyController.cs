using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
namespace SizeUp.Web.Controllers
{
    public class CompanyController : BaseController
    {
        //
        // GET: /Company/

        public ActionResult Team()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Team.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult Benefits()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };

            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Benefits.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult About()
        {

            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };

            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "About.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }

        public ActionResult Mission()
        {
            return View();
        }

        public ActionResult Jobs()
        {

            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };

            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Content = context.ResourceStrings.Where(i => i.Name == "Jobs.Content").Select(i => i.Value).FirstOrDefault();
            }
            return View();
        }
    }
}
