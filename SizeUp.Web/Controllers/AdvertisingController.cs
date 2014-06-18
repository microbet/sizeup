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
            if (CurrentInfo.CurrentPlace.Id == null || CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            ViewBag.Header.ActiveTab = NavItems.Advertising;
            return View();
        }

    }
}
