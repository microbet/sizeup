using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class RevenueController : Controller
    {
        //
        // GET: /Api/Revenue/

        public ActionResult Revenue(long cityd)
        {
            return View();
        }

        public ActionResult Percentile(long cityd)
        {
            return View();
        }


        public ActionResult BandsByZip(string boundingEntityId)
        {
            return View();
        }

        public ActionResult BandsByCounty(string boundingEntityId)
        {
            return View();
        }

        public ActionResult BandsByState()
        {
            return View();
        }


        

    }
}
