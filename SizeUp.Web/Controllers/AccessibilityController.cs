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
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Configuration;

namespace SizeUp.Web.Controllers
{
    public class AccessibilityController : BaseController
    {
        //
        // GET: /Accessibility/
        
        
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

        public ActionResult Revenue(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.Attribute = "Average Business Annual Revenue";
                return View("Heatmap");
            }
        }

        public static IQueryable<Data.ConsumerExpenditureVariable> Variables(SizeUpContext context)
        {
            var data = context.ConsumerExpenditureVariables;
            return data;
        }

        public static IQueryable<Data.ConsumerExpenditure> Get(SizeUpContext context)
        {
            var data = context.ConsumerExpenditures.Where(i => i.Year == CommonFilters.TimeSlice.ConsumerExpenditures.Year && i.Quarter == CommonFilters.TimeSlice.ConsumerExpenditures.Quarter);
            return data;
        }

        public ActionResult Competition(long variableId, long boundingGeographicLocationId, int bands, Core.DataLayer.Granularity granularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var variable = Variables(context).Where(i => i.Id == variableId).Select(i => i.Variable).FirstOrDefault();
                var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);

                int granularityId = 0;
                if (granularity == Core.DataLayer.Granularity.ZipCode)
                {
                    granularityId = 1;
                }
                else if (granularity == Core.DataLayer.Granularity.City)
                {
                    granularityId = 2;
                }
                else if (granularity == Core.DataLayer.Granularity.County)
                {
                    granularityId = 3;
                }
                else if (granularity == Core.DataLayer.Granularity.Place)
                {
                    granularityId = 4;
                }
                else if (granularity == Core.DataLayer.Granularity.Metro)
                {
                    granularityId = 5;
                }
                else if (granularity == Core.DataLayer.Granularity.State)
                {
                    granularityId = 6;
                }
                else if (granularity == Core.DataLayer.Granularity.Nation)
                {
                    granularityId = 7;
                }

                var data = Get(context)
                    .Where(i => i.GeographicLocation.Granularity.Name == gran)
                    .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

                int count = data.ToList().Count();

                string queryString = string.Format("select LongName, {0} " +
                                                    "from dbo.ConsumerExpenditures as ce " +
                                                    "join dbo.GeographicLocation as gl on ce.GeographicLocationId = gl.Id " +
                                                    "where ce.GeographicLocationId in ( " +
                                                    "SELECT gl.GeographicLocationId " +
                                                    "FROM dbo.GeographicLocation as g " +
                                                    "join dbo.GeographicLocationGeographicLocation as gl on g.Id = gl.GeographicLocationId " +
                                                    "where GranularityId={1} and gl.IntersectedGeographicLocationId = {2})", variable, granularityId, boundingGeographicLocationId);
                var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["SizeUpContext"].ConnectionString);
                List<Payload> t = new List<Payload>();
                using (SqlConnection connection = new SqlConnection(conn.StoreConnection.ConnectionString))
                {
                    SqlCommand command =
                        new SqlCommand(queryString, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data. 
                    while (reader.Read())
                    {
                        t.Add(new Payload() { Name = reader[0].ToString(), Value = Convert.ToInt64(reader[1]) });
                    }

                    // Call Close when done reading.
                    reader.Close();
                }

                var output = t
                .NTileDescending(i => i.Value, bands)
                   .Select(i => new Band
                   {
                       Min = string.Format("${0}", Format(i.Min(v => v.Value))),
                       Max = string.Format("${0}", Format(i.Max(v => v.Value))),
                       Items = i.Select(v => v.Name).ToList()
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
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Competition";
                return View("Heatmap");
            }
            
        }

        public class Payload
        {
            public string Name { get; set; }
            public long Value { get; set; }

        }

        public ActionResult AverageSalary(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Average Salary";
                return View("Heatmap");
            }
        }


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


        public ActionResult EmployeesPerCapita(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Employees Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult CostEffectiveness(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Cost Effectiveness";
                return View("Heatmap");
            }
        }


        public ActionResult RevenuePerCapita(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Revenue Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult TotalRevenue(int bands, int industryId, long boundingGeographicLocationId, Core.DataLayer.Granularity granularity)
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
                ViewBag.BoundingEntity = context.GeographicLocations.Where(i => i.Id == boundingGeographicLocationId).Select(i => i.LongName).FirstOrDefault();
                ViewBag.Attribute = "Total Revenue";
                return View("Heatmap");
            }
        }

        public ActionResult YearStarted(int placeId, int startYear, int endYear, int industryId)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = Core.DataLayer.Place.Get(context, placeId);

                var c = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.City.Id, startYear, endYear);
                var co = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.County.Id, startYear, endYear);
                var s = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.State.Id, startYear, endYear);
                var n = Core.DataLayer.YearStarted.Chart(context, industryId, place.Nation.Id, startYear, endYear);

                List<Web.Models.Accessibility.Table> data =
                    c.Join(co, i => i.Key, o => o.Key, (i, o) => new { City = o, County = i })
                    .Join(s, i => i.City.Key, o => o.Key, (i, o) => new { City = i.City, County = i.County, State = o })
                    .Join(n, i => i.City.Key, o => o.Key, (i, o) => new Web.Models.Accessibility.Table() { Year = i.City.Key, City = i.City.Value, County = i.County.Value, State = i.State.Value, Nation = o.Value })
                    .ToList();

                if (place.Metro.Id.HasValue)
                {
                    var m = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.Metro.Id, startYear, endYear);
                    data = data.Join(m, i => i.Year, o => o.Key, (i, o) => new Web.Models.Accessibility.Table() { Year = i.Year, City = i.City, County = i.County, State = i.State, Nation = i.Nation, Metro = o.Value })
                        .ToList();
                }
                ViewBag.Data = data;

                return View("Linechart");
            }
        }
    }
}
