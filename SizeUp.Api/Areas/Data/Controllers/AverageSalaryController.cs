using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class AverageSalaryController : BaseController
    {
        //
        // GET: /Api/AverageSalary/

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(int industryId, int geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageSalary.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentage(int industryId, int geographicLocationId, int value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.AverageSalary.Percentage(context, industryId, geographicLocationId, value);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Bands(
            long industryId, long boundingGeographicLocationId, int bands,
            Core.DataLayer.Granularity granularity, string contentType = "*/*"
        ) {
            if ("text/html".Equals(contentType))
            {
                using (var context = ContextFactory.SizeUpContext)
                {
                    Expression<Func<SizeUp.Data.IndustryData, bool>> filter = i => i.AverageAnnualSalary != null;
                    Expression<Func<SizeUp.Data.IndustryData, Kpi.LabeledValue>> selector;
                    selector = i => new Kpi.LabeledValue
                    {
                        Label = i.GeographicLocation.LongName,
                        Value = i.AverageAnnualSalary
                    };
                    Kpi.GetKpiModel(
                        ViewBag, context,
                        industryId, boundingGeographicLocationId, granularity,
                        filter, selector, "Average Salary", "${0}", bands
                    );
                    return View("Heatmap");
                }
            }
            else
            {
                using (var context = ContextFactory.SizeUpContext)
                {
                    var data = Core.DataLayer.AverageSalary.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
