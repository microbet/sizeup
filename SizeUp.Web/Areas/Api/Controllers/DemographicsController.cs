using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class DemographicsController : Controller
    {
        //
        // GET: /Api/Demographics/

        public ActionResult Demographics(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, id).FirstOrDefault();
                //IQueryable<Models.AverageRevenue.ChartItem> m = null;

                var data = new Models.Demographics.Place()
                {
                    PlaceId = locations.Id
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
