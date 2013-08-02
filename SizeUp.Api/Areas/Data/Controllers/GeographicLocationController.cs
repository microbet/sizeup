using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;


namespace SizeUp.Api.Areas.Data.Controllers
{
    public class GeographicLocationController : BaseController
    {
        //
        // GET: /Data/GeographicLocation/

        [APIAuthorize(Role = "IndustryData")]
        public ActionResult BestIndustries(long geographicLocationId, string attribute = "TotalRevenue")
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.GeographicLocation.Ranks(context);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
