using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.Extensions;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
using System.Linq.Expressions;

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
        public ActionResult Bands(long industryId, long boundingGeographicLocationId, int bands, Core.DataLayer.Granularity granularity, string contentType = "*/*")
        {
            // contentType arg should also be checked for application/json and application/javascript,
            // but those seem to be inferred by other code like APIContext.IsJsonp, alluded to in
            // Controller.Json, and etc. For now we'll leave those alone and just check for:
            if ("text/html".Equals(contentType))
            {
                using (var context = ContextFactory.SizeUpContext)
                {
                    Expression<Func<SizeUp.Data.IndustryData, bool>> filter = i => i.AverageRevenue != null;
                    Expression<Func<SizeUp.Data.IndustryData, Kpi.LabeledValue>> selector;
                    selector = i => new Kpi.LabeledValue
                    {
                        Label = i.GeographicLocation.LongName,
                        Value = i.AverageRevenue
                    };
                    Kpi.GetKpiModel(
                        ViewBag, context,
                        industryId, boundingGeographicLocationId, granularity,
                        filter, selector, "Average Annual Revenue", bands
                    );
                    return View("Heatmap");
                }
            }
            else
            {
                using (var context = ContextFactory.SizeUpContext)
                {
                    var data = Core.DataLayer.AverageRevenue.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }


    }
}
