using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Types;
using System.Data.Spatial;
using SizeUp.Data;
using SizeUp.Core.Geo;
using SizeUp.Core;
using SizeUp.Core.Extensions;
using System.Drawing;

using SizeUp.Core.DataLayer;

namespace SizeUp.Web.Controllers
{
    public class AccessibilityController : BaseController
    {
        //
        // GET: /Accessibility/

        protected string GetBoundingEntityName(SizeUpContext context, long placeId, Core.DataLayer.Granularity boundingGranularity)
        {
            string name = "";
            var place = Core.DataLayer.Place.Get(context, placeId);
            if (boundingGranularity == Core.DataLayer.Granularity.County)
            {
                name = place.County.Name + " County, " + place.State.Abbreviation;
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Metro)
            {
                name = place.Metro.Name;
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.State)
            {
                name = place.State.Name;
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Nation)
            {
                name = "USA";
            }
            return name;
        }

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

        public ActionResult Revenue(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Business Annual Revenue";
                return View("Heatmap");
            }
        }

        public ActionResult AverageSalary(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.AverageAnnualSalary
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Salary";
                return View("Heatmap");
            }
        }


        public ActionResult AverageEmployees(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Employees Per Business";
                return View("Heatmap");
            }
        }


        public ActionResult EmployeesPerCapita(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.EmployeesPerCapita
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Employees Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult CostEffectiveness(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.CostEffectiveness
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Cost Effectiveness";
                return View("Heatmap");
            }
        }


        public ActionResult RevenuePerCapita(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.RevenuePerCapita
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Revenue Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult TotalRevenue(int bands, int industryId, long placeId, Core.DataLayer.Granularity granularity, Core.DataLayer.Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                   .Where(i => i.IndustryId == industryId)
                   .Select(i => new
                   {
                       Label = i.GeographicLocation.LongName,
                       Value = i.TotalRevenue
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
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Total Revenue";
                return View("Heatmap");
            }
        }

        public ActionResult YearStarted(int placeId, int startYear, int endYear,  int industryId)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Granularity.City);
                var co = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Granularity.County);
                var m = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Granularity.Metro);
                var s = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Granularity.State);
                var n = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Granularity.Nation);

                List<Web.Models.Accessibility.Table> data =
                    c.Join(co, i => i.Key, o => o.Key, (i, o) => new { City = o, County = i })
                    .Join(s, i => i.City.Key, o => o.Key, (i, o) => new { City = i.City, County = i.County, State = o })
                    .Join(n, i => i.City.Key, o => o.Key, (i, o) => new Web.Models.Accessibility.Table() { Year = i.City.Key, City = i.City.Value, County = i.County.Value, State = i.State.Value, Nation = o.Value })
                    .ToList();

                if (context.Places.Any(cm=>cm.Id == placeId && cm.County.Metro != null))
                {
                    data = data.Join(m, i => i.Year, o => o.Key, (i, o) => new Web.Models.Accessibility.Table() { Year = i.Year, City = i.City, County = i.County, State = i.State, Nation = i.Nation, Metro = o.Value })
                        .ToList();
                }
                ViewBag.Data = data;

                return View("Linechart");
            }
        }
    }
}
