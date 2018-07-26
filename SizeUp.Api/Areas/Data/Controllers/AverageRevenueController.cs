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

namespace SizeUp.Api.Areas.Data.Controllers
{

    public class AverageRevenueController : BaseController
    {
        // Band, FormatBands, and Format are salvaged (copied) from SizeUp.Web (AccessibilityController.cs).
        // To be moved into their own namespace if the salvage operation works.

        public class Band
        {
            public string Min { get; set; }
            public string Max { get; set; }
            public List<string> Items { get; set; }
            
        }

        protected List<Band> FormatBands(List<Band> bands)
        {
            Band old = null;
            foreach (var band in bands)
            {
                if (old != null)
                {
                    old.Max = band.Min;
                }
                old = band;
            }
            return bands;
        }

        protected string Format(double val)
        {
            string output = "";
            if (val < 10)
            {
                output = string.Format("{0:0.0}", System.Math.Round(val, 1));
            }
            else if (val >= 10 && val < 10000)
            {
                output = string.Format("{0:0}", System.Math.Round(val, 1));
            }
            else if (val >= 10000 && val < 1000000)
            {
                output = string.Format("{0:0.0}K", System.Math.Round((val / 1000), 1));
            }
            else if (val >= 1000000 && val < 1000000000)
            {
                output = string.Format("{0:0.0}M", System.Math.Round((val / 1000000), 1));
            }
            else if (val >= 1000000000)
            {
                output = string.Format("{0:0.0}B", System.Math.Round((val / 1000000000), 1));
            }
            return output;
        }

        protected string Format(long val)
        {
            string output = "";
            if (val < 10)
            {
                output = string.Format("{0:0}", val);
            }
            else if (val >= 10 && val < 10000)
            {
                output = string.Format("{0:0}",val);
            }
            else if (val >= 10000 && val < 1000000)
            {
                output = string.Format("{0:0.0}K", System.Math.Round(((double)val / 1000), 1));
            }
            else if (val >= 1000000 && val < 1000000000)
            {
                output = string.Format("{0:0.0}M", System.Math.Round(((double)val / 1000000), 1));
            }
            else if (val >= 1000000000)
            {
                output = string.Format("{0:0.0}B", System.Math.Round(((double)val / 1000000000), 1));
            }
            return output;
        }

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
                var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

                using (var context = ContextFactory.SizeUpContext)
                {
                    var data = Core.DataLayer.IndustryData.Get(context)
                        .Where(i => i.GeographicLocation.GeographicLocations.Any(gl => gl.Id == boundingGeographicLocationId))
                       .Where(i => i.IndustryId == industryId)
                       .Where(i => i.GeographicLocation.Granularity.Name == gran)
                       .Select(i => new
                       {
                           Label = i.GeographicLocation.LongName,
                           Value = i.AverageRevenue
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
                    ViewBag.Area = ViewBag.BoundingEntity;
                    ViewBag.Attribute = "Average Business Annual Revenue";
                    ViewBag.Industry = context.Industries
                        .Where(i => i.Id == industryId)
                        .Select(i => i.Name)
                        .FirstOrDefault();
                    ViewBag.Kpi = "Average Annual Revenue";
                    ViewBag.Q = bands.ToString();
                    ViewBag.ThemeUrl = System.Configuration.ConfigurationManager.AppSettings["Theme.Url"];

                    ViewBag.Query = string.Format(
                        "Rank each {0} in {1}, by {2} of {3} businesses, in {4} quantiles",
                        ViewBag.LevelOfDetail, ViewBag.Area, ViewBag.Kpi, ViewBag.Industry, ViewBag.Q
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
