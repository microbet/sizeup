using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core;
using SizeUp.Data;

namespace SizeUp.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(long id, string attribute = "TotalRevenue", int count = 15)
        {

            using (var context = ContextFactory.SizeUpContext)
            {
                //var x = Core.DataLayer.Place.ListNear(context, new Core.Geo.LatLng() { Lat = 37.7750, Lng = -122.4183 }).OrderBy(i=>i.Distance).Take(5).ToList(); ;
                /*              
               --sf 82478
               --napa 82242
               --la 82414
               --boston 91539
               --detroit 91814
               --santa cruz  82481
               --las vegas 95712
               --emeryville 82472
               --salem,ma 91543
               -- oakland 82467
               -- new york 97017
               */

                var x = Core.DataLayer.GeographicLocation.BestIndustries(context, id, attribute).Take(count).ToList();


                return Json(x, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Test()
        {

            using (var context = ContextFactory.SizeUpContext)
            {
                var x = Core.DataLayer.Business.Get(context)
                    .Where(i => i.IndustryId == 9409)
                    .Where(i => i.Cities.Any(c => c.Id == 97017))
                    .Select(new Core.DataLayer.Projections.Business.Default().Expression)
                    .ToList();

                ViewBag.Businesses = x;
                return View();
            }

        }

    }
}
