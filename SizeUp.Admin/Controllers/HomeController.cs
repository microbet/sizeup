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

        public ActionResult Index()
        {

            using (var context = ContextFactory.SizeUpContext)
            {
                //var x = Core.DataLayer.Place.ListNear(context, new Core.Geo.LatLng() { Lat = 37.7750, Lng = -122.4183 }).OrderBy(i=>i.Distance).Take(5).ToList(); ;

                var x = Core.DataLayer.AverageRevenue.Percentile(context, 8589, 3051, 1000000, Core.DataLayer.Granularity.County);

                return Json(x, JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}
