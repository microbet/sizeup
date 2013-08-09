using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class TotalRevenueController : BaseController
    {
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.TotalRevenue.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Bands(long industryId, long boundingGeographicLocationId, int bands, Core.DataLayer.Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.TotalRevenue.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
