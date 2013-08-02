using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class AverageRevenueController : BaseController
    {
        //
        // GET: /Api/AverageRevenue/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long placeId, Core.DataLayer.Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Chart(context, industryId, placeId, granularity);               
                return Json(data, JsonRequestBehavior.AllowGet);         
            }
        }


        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long placeId, long value, Core.DataLayer.Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Percentile(context, industryId, placeId, value, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Bands(long industryId, long placeId, int bands, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity = Core.DataLayer.Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);
                return Json(data, JsonRequestBehavior.AllowGet); 
            }
        }


    }
}
