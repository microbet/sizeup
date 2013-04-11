using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
namespace SizeUp.Web.Controllers
{
    public class DevelopersController : BaseController
    {

        public ActionResult Index()
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            return View();
        }


        public ActionResult Documentation()
        {
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            return View();
        }

        

    }
}
