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
using System.Linq.Expressions;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class CostEffectivenessController : BaseController
    {
        //
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(int industryId, int geographicLocationId )
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.CostEffectiveness.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentage(int industryId, int geographicLocationId, int revenue, int employees, int salary)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var ce = revenue / (double)(employees * salary);
                var obj = Core.DataLayer.CostEffectiveness.Percentage(context, industryId, geographicLocationId, ce);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Bands(
            long industryId, long boundingGeographicLocationId, int bands,
            Core.DataLayer.Granularity granularity, string contentType = "*/*"
        ) {
            using (var context = ContextFactory.SizeUpContext)
            {
                if ("text/html".Equals(contentType))
                {
                    Expression<Func<SizeUp.Data.IndustryData, bool>> filter = i => i.CostEffectiveness != null;
                    Expression<Func<SizeUp.Data.IndustryData, Kpi.LabeledValue>> selector;
                    selector = i => new Kpi.LabeledValue
                    {
                        Label = i.GeographicLocation.LongName,
                        Value = i.CostEffectiveness
                    };
                    Kpi.GetKpiModel(
                        ViewBag, context,
                        industryId, boundingGeographicLocationId, granularity,
                        filter, selector, "Cost Effectiveness", "{0}", bands
                    );
                    return View("Heatmap");
                }
                else
                {
                    var data = Core.DataLayer.CostEffectiveness.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
