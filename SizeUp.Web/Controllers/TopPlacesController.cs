using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
namespace SizeUp.Web.Controllers
{
    public class TopPlacesController : BaseController
    {
        //
        // GET: /Competition/

        public ActionResult Index()
        {
            ViewBag.Header.ActiveTab = NavItems.TopPlaces;
            return View();
        }

    }
}
