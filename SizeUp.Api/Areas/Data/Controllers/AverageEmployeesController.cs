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
    public class AverageEmployeesController : BaseController
    {
        //
        // GET: /Api/Employee/


        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageEmployees.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, long value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageEmployees.Percentile(context, industryId, geographicLocationId, value);
                return Json(data, JsonRequestBehavior.AllowGet);
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
                    Expression<Func<SizeUp.Data.IndustryData, bool>> filter = i => i.AverageEmployees != null;
                    Expression<Func<SizeUp.Data.IndustryData, Kpi.LabeledValue>> selector;
                    selector = i => new Kpi.LabeledValue
                    {
                        Label = i.GeographicLocation.LongName,
                        Value = i.AverageEmployees
                    };
                    Kpi.GetKpiModel(
                        ViewBag, context,
                        industryId, boundingGeographicLocationId, granularity,
                        filter, selector, "Average Employees Per Business", "{0}", bands
                    );
                    return View("Heatmap");
                }
                else
                {
                    var data = Core.DataLayer.AverageEmployees.Bands(context, industryId, boundingGeographicLocationId, bands, granularity);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /*
        public ActionResult AverageEmployees(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context)
                    .Where(i=> i.GeographicLocation.GeographicLocations.Any(gl=>gl.Id == boundingGeographicLocationId))
                   .Where(i => i.IndustryId == industryId)
                   .Where(i=>i.GeographicLocation.Granularity.Name == gran)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.AverageEmployees
                   })
                   .ToList()
                   .NTileDescending(i => i.Value, bands)
                   .Select(i => new Band
                   {
                       Min = string.Format("${0}", Format(i.Min(v => v.Value.Value))),
                       Max = string.Format("${0}", Format(i.Max(v => v.Value.Value))),
                       Items = i.Select(v => v.Label).ToList()
                   })
                   .ToList();

                if (granularity == Core.DataLayer.Granularity.ZipCode)
                {
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (granularity == Core.DataLayer.Granularity.County)
                {
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Core.DataLayer.Granularity.State)
                {
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(data);
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Average Employees Per Business";
                return View("Heatmap");
            }
        }
         */
    }
}
