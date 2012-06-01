using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
namespace SizeUp.Web.Controllers
{
    public class AdvertisingController : BaseController
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
            ViewBag.Header.ActiveTab = NavItems.Advertising;
            return View();
        }

    }
}
