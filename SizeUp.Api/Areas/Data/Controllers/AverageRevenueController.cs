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
        public ActionResult Chart(long industryId, long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Chart(context, industryId, geographicLocationId);               
                return Json(data, JsonRequestBehavior.AllowGet);         
            }
        }


        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, long value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Percentile(context, industryId, geographicLocationId, value);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Bands(long industryId, long boundingGeographicLocationId, int bands, Core.DataLayer.Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageRevenue.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                return Json(data, JsonRequestBehavior.AllowGet); 
            }
        }


    }
}
