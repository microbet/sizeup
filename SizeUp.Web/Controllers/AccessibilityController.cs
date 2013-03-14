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
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Web.Controllers
{
    public class AccessibilityController : BaseController
    {
        //
        // GET: /Accessibility/

        protected string GetBoundingEntityName(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            string name = "";
            var place = Core.DataLayer.Place.Get(context, placeId);
            if (boundingGranularity == Granularity.County)
            {
                name = place.County.Name + " County, " + place.State.Abbreviation;
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                name = place.Metro.Name;
            }
            else if (boundingGranularity == Granularity.State)
            {
                name = place.State.Name;
            }
            else if (boundingGranularity == Granularity.Nation)
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

        public ActionResult Revenue(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var zips = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.AverageRevenue.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);

                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i=>i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.ZipCode)
                {
                    var entity = zips.Join(zipData, i => i.Id, o => o.ZipCodeId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageRevenue && i.Max >= e.Data.AverageRevenue).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if(granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageRevenue && i.Max >= e.Data.AverageRevenue).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if(granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageRevenue && i.Max >= e.Data.AverageRevenue).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Business Annual Revenue";
                return View("Heatmap");
            }
        }

        public ActionResult AverageSalary(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.AverageSalary.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);


                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageAnnualSalary && i.Max >= e.Data.AverageAnnualSalary).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageAnnualSalary && i.Max >= e.Data.AverageAnnualSalary).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Salary";
                return View("Heatmap");
            }
        }


        public ActionResult AverageEmployees(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var zips = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.AverageEmployees.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);


                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.ZipCode)
                {
                    var entity = zips.Join(zipData, i => i.Id, o => o.ZipCodeId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0}", Format(i.Min)),
                        Max = string.Format("{0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageEmployees && i.Max >= e.Data.AverageEmployees).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0}", Format(i.Min)),
                        Max = string.Format("{0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageEmployees && i.Max >= e.Data.AverageEmployees).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0}", Format(i.Min)),
                        Max = string.Format("{0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.AverageEmployees && i.Max >= e.Data.AverageEmployees).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Average Employees Per Business";
                return View("Heatmap");
            }
        }


        public ActionResult EmployeesPerCapita(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var zips = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.EmployeesPerCapita.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);


                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.ZipCode)
                {
                    var entity = zips.Join(zipData, i => i.Id, o => o.ZipCodeId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0:G3}", Format(i.Min)),
                        Max = string.Format("{0:G3}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.EmployeesPerCapita && i.Max >= e.Data.EmployeesPerCapita).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0:G3}", Format(i.Min)),
                        Max = string.Format("{0:G3}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.EmployeesPerCapita && i.Max >= e.Data.EmployeesPerCapita).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0:G3}", Format(i.Min)),
                        Max = string.Format("{0:G3}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.EmployeesPerCapita && i.Max >= e.Data.EmployeesPerCapita).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Employees Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult CostEffectiveness(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.CostEffectiveness.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);



                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0:G3}", Format(i.Min)),
                        Max = string.Format("{0:G3}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.CostEffectiveness && i.Max >= e.Data.CostEffectiveness).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("{0:G3}", Format(i.Min)),
                        Max = string.Format("{0:G3}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.CostEffectiveness && i.Max >= e.Data.CostEffectiveness).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Cost Effectiveness";
                return View("Heatmap");
            }
        }


        public ActionResult RevenuePerCapita(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var zips = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.RevenuePerCapita.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);


                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);

                List<Band> output = new List<Band>();
                if (granularity == Granularity.ZipCode)
                {
                    var entity = zips.Join(zipData, i => i.Id, o => o.ZipCodeId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.RevenuePerCapita && i.Max >= e.Data.RevenuePerCapita).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.RevenuePerCapita && i.Max >= e.Data.RevenuePerCapita).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.RevenuePerCapita && i.Max >= e.Data.RevenuePerCapita).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
                ViewBag.BoundingEntity = GetBoundingEntityName(context, placeId, boundingGranularity);
                ViewBag.Attribute = "Revenue Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult TotalRevenue(int bands, int industryId, long placeId, Granularity granularity, Granularity boundingGranularity)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                var zips = Core.DataLayer.Base.ZipCode.In(context, placeId, boundingGranularity);
                var counties = Core.DataLayer.Base.County.In(context, placeId, boundingGranularity);
                var states = Core.DataLayer.Base.State.In(context, placeId, boundingGranularity);

                var data = Core.DataLayer.TotalRevenue.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);


                var zipData = Core.DataLayer.Base.IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                var countyData = Core.DataLayer.Base.IndustryData.County(context).Where(i => i.IndustryId == industryId);
                var stateData = Core.DataLayer.Base.IndustryData.State(context).Where(i => i.IndustryId == industryId);


                List<Band> output = new List<Band>();
                if (granularity == Granularity.ZipCode)
                {
                    var entity = zips.Join(zipData, i => i.Id, o => o.ZipCodeId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.TotalRevenue && i.Max >= e.Data.TotalRevenue).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (granularity == Granularity.County)
                {
                    var entity = counties.Join(countyData, i => i.Id, o => o.CountyId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.TotalRevenue && i.Max >= e.Data.TotalRevenue).Select(e => e.Entity.Name + " County, " + e.Entity.State.Abbreviation).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "County";
                }
                else if (granularity == Granularity.State)
                {
                    var entity = states.Join(stateData, i => i.Id, o => o.StateId, (i, o) => new { Entity = i, Data = o });
                    output = data.Select(i => new Band
                    {
                        Min = string.Format("${0}", Format(i.Min)),
                        Max = string.Format("${0}", Format(i.Max)),
                        Items = entity.Where(e => i.Min <= e.Data.TotalRevenue && i.Max >= e.Data.TotalRevenue).Select(e => e.Entity.Name).ToList()
                    }).ToList();
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.Bands = FormatBands(output);
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
                var c = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Base.Granularity.City);
                var co = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Base.Granularity.County);
                var m = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Base.Granularity.Metro);
                var s = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Base.Granularity.State);
                var n = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, Core.DataLayer.Base.Granularity.Nation);

                List<Web.Models.Accessibility.Table> data =
                    c.Join(co, i => i.Key, o => o.Key, (i, o) => new { City = o, County = i })
                    .Join(s, i => i.City.Key, o => o.Key, (i, o) => new { City = i.City, County = i.County, State = o })
                    .Join(n, i => i.City.Key, o => o.Key, (i, o) => new Web.Models.Accessibility.Table() { Year = i.City.Key, City = i.City.Value, County = i.County.Value, State = i.State.Value, Nation = o.Value })
                    .ToList();

                if (context.CityCountyMappings.Any(cm=>cm.Id == placeId && cm.County.Metro != null))
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
