using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Types;
using System.Data.Spatial;
using SizeUp.Data;
using SizeUp.Core.Geo;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Extensions;
using System.Drawing;


namespace SizeUp.Web.Controllers
{
    public class AccessibilityController : BaseController
    {
        //
        // GET: /Accessibility/
        protected BoundingEntity GetBoundingEntity()
        {
            var boundingEntityId = HttpContext.Request["boundingEntityId"];
            BoundingEntity boundingEntity = null;
            if (!string.IsNullOrEmpty(boundingEntityId))
            {
                boundingEntity = new BoundingEntity(boundingEntityId);
            }
            return boundingEntity;
        }

        protected string GetBoundingEntityName(SizeUpContext context)
        {
            var be = GetBoundingEntity();
            string name = "USA";
            if (be != null && be.EntityType == BoundingEntity.BoundingEntityType.City)
            {
                name = context.Cities.Where(i => i.Id == be.EntityId).Select(i => i.Name + ", " + i.State.Abbreviation).FirstOrDefault();
            }
            else if (be != null && be.EntityType == BoundingEntity.BoundingEntityType.County)
            {
                name = context.Counties.Where(i => i.Id == be.EntityId).Select(i => i.Name + " County, " + i.State.Abbreviation).FirstOrDefault();
            }
            else if (be != null && be.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                name = context.Metroes.Where(i => i.Id == be.EntityId).Select(i => i.Name).FirstOrDefault();
            }
            else if (be != null && be.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                name = context.States.Where(i => i.Id == be.EntityId).Select(i => i.Name).FirstOrDefault();
            }
            return name;
        }


        protected BoundingBox GetBoundingBox()
        {
            var southWest = HttpContext.Request["southWest"].Split(',');
            var northEast = HttpContext.Request["northEast"].Split(',');
            BoundingBox box = new BoundingBox(new PointF(float.Parse(southWest[1]), float.Parse(southWest[0])), new PointF(float.Parse(northEast[1]), float.Parse(northEast[0])));
            return box;
        }

        public class Band
        {
            public string Min { get; set; }
            public string Max { get; set; }
            //public string Template { get; set; }
            //public string FormattedMin { get { return string.Format(Template, Format(Min)); } }
            //public string FormattedMax { get { return string.Format(Template, Format(Max)); } }
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



        protected IQueryable<IndustryDataByZip> GetZipCodeData(SizeUpContext context, int industryId)
        {
            var boundingEntity = GetBoundingEntity();

            IQueryable<long> ids = ZipCodes.GetBounded(context, boundingEntity)
                .Select(i => i.Id);

            var data = IndustryData.GetZipCodes(context, industryId)
                .Join(ids, i => i.ZipCodeId, i => i, (i, o) => i);

            return data;
        }


        protected IQueryable<long> GetZipsInBounds(SizeUpContext context)
        {
            var boundingBox = GetBoundingBox();
            var boundingEntity = GetBoundingEntity();
            IQueryable<long> ids = ZipCodes.GetBounded(context, boundingEntity)
                .Select(i => i.Id);
            var entities = Core.DataAccess.Geography.GetBoundingBoxedZips(context, boundingBox)
                .Join(ids, i => i.ZipCodeId, o => o, (i, o) => o);

            return entities;
        }

        protected IQueryable<IndustryDataByCounty> GetCountyData(SizeUpContext context, int industryId)
        {
            var boundingEntity = GetBoundingEntity();

            IQueryable<long> ids = Counties.GetBounded(context, boundingEntity)
                .Select(i => i.Id);

            var data = IndustryData.GetCounties(context, industryId)
                .Join(ids, i => i.CountyId, i => i, (i, o) => i);

            return data;
        }


        protected IQueryable<long> GetCountiesInBounds(SizeUpContext context)
        {
            var boundingBox = GetBoundingBox();
            var boundingEntity = GetBoundingEntity();
            IQueryable<long> ids = Counties.GetBounded(context, boundingEntity)
                .Select(i => i.Id);
            var entities = Core.DataAccess.Geography.GetBoundingBoxedCounties(context, boundingBox)
                .Join(ids, i => i.CountyId, o => o, (i, o) => o);

            return entities;
        }


        protected IQueryable<IndustryDataByState> GetStateData(SizeUpContext context, int industryId)
        {
            var boundingEntity = GetBoundingEntity();

            IQueryable<long> ids = States.GetBounded(context, boundingEntity)
                .Select(i => i.Id);

            var data = IndustryData.GetStates(context, industryId)
                .Join(ids, i => i.StateId, i => i, (i, o) => i);

            return data;
        }


        protected IQueryable<long> GetStatesInBounds(SizeUpContext context)
        {
            var boundingBox = GetBoundingBox();
            var boundingEntity = GetBoundingEntity();
            IQueryable<long> ids = States.GetBounded(context, boundingEntity)
                .Select(i => i.Id);
            var entities = Core.DataAccess.Geography.GetBoundingBoxedStates(context, boundingBox)
                .Join(ids, i => i.StateId, o => o, (i, o) => o);

            return entities;
        }



        public ActionResult Revenue(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "zip")
                {
                    var entities = GetZipsInBounds(context).ToList();

                    var data = GetZipCodeData(context, industryId)
                        .Where(i => i.AverageRevenue > 0)
                        .Select(i => new { i.AverageRevenue, i.ZipCode })
                        .ToList()
                        .NTile(i => i.AverageRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.AverageRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.AverageRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.ZipCode.Id)).Select(d => d.ZipCode.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.AverageRevenue > 0)
                        .Select(i => new { i.AverageRevenue, i.County })
                        .ToList()
                        .NTile(i => i.AverageRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.AverageRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.AverageRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.AverageRevenue > 0)
                        .Select(i => new { i.AverageRevenue, i.State })
                        .ToList()
                        .NTile(i => i.AverageRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.AverageRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.AverageRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Average Business Annual Revenue";
                return View("Heatmap");
            }
        }

        public ActionResult AverageSalary(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.AverageAnnualSalary > 0)
                        .Select(i => new { i.AverageAnnualSalary, i.County })
                        .ToList()
                        .NTile(i => i.AverageAnnualSalary, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.AverageAnnualSalary.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.AverageAnnualSalary.Value))),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.AverageAnnualSalary > 0)
                        .Select(i => new { i.AverageAnnualSalary, i.State })
                        .ToList()
                        .NTile(i => i.AverageAnnualSalary, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.AverageAnnualSalary.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.AverageAnnualSalary.Value))),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Average Salary";
                return View("Heatmap");
            }
        }


        public ActionResult AverageEmployees(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "zip")
                {
                    var entities = GetZipsInBounds(context).ToList();

                    var data = GetZipCodeData(context, industryId)
                        .Where(i => i.AverageEmployees > 0)
                        .Select(i => new { i.AverageEmployees, i.ZipCode })
                        .ToList()
                        .NTile(i => i.AverageEmployees, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0}", Format(i.Min(d => d.AverageEmployees.Value))),
                            Max = string.Format("{0}", Format(i.Max(d => d.AverageEmployees.Value))),
                            Items = i.Where(d => entities.Contains(d.ZipCode.Id)).Select(d => d.ZipCode.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.AverageEmployees > 0)
                        .Select(i => new { i.AverageEmployees, i.County })
                        .ToList()
                        .NTile(i => i.AverageEmployees, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0}", Format(i.Min(d => d.AverageEmployees.Value))),
                            Max = string.Format("{0}", Format(i.Max(d => d.AverageEmployees.Value))),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.AverageEmployees > 0)
                        .Select(i => new { i.AverageEmployees, i.State })
                        .ToList()
                        .NTile(i => i.AverageEmployees, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0}", Format(i.Min(d => d.AverageEmployees.Value))),
                            Max = string.Format("{0}", Format(i.Max(d => d.AverageEmployees.Value))),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Average Employees Per Business";
                return View("Heatmap");
            }
        }


        public ActionResult EmployeesPerCapita(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "zip")
                {
                    var entities = GetZipsInBounds(context).ToList();

                    var data = GetZipCodeData(context, industryId)
                        .Where(i => i.EmployeesPerCapita > 0)
                        .Select(i => new { i.EmployeesPerCapita, i.ZipCode })
                        .ToList()
                        .NTile(i => i.EmployeesPerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0:G3}", i.Min(d => d.EmployeesPerCapita.Value)),
                            Max = string.Format("{0:G3}", i.Max(d => d.EmployeesPerCapita.Value)),
                            Items = i.Where(d => entities.Contains(d.ZipCode.Id)).Select(d => d.ZipCode.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.EmployeesPerCapita > 0)
                        .Select(i => new { i.EmployeesPerCapita, i.County })
                        .ToList()
                        .NTile(i => i.EmployeesPerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0:G3}", i.Min(d => d.EmployeesPerCapita.Value)),
                            Max = string.Format("{0:G3}", i.Max(d => d.EmployeesPerCapita.Value)),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.EmployeesPerCapita > 0)
                        .Select(i => new { i.EmployeesPerCapita, i.State })
                        .ToList()
                        .NTile(i => i.EmployeesPerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0:G3}", i.Min(d => d.EmployeesPerCapita.Value)),
                            Max = string.Format("{0:G3}", i.Max(d => d.EmployeesPerCapita.Value)),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Employees Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult CostEffectiveness(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
               if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.CostEffectiveness > 0)
                        .Select(i => new { i.CostEffectiveness, i.County })
                        .ToList()
                        .NTile(i => i.CostEffectiveness, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0:G3}", i.Min(d => d.CostEffectiveness.Value)),
                            Max = string.Format("{0:G3}", i.Max(d => d.CostEffectiveness.Value)),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.CostEffectiveness > 0)
                        .Select(i => new { i.CostEffectiveness, i.State })
                        .ToList()
                        .NTile(i => i.CostEffectiveness, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("{0:G3}", i.Min(d => d.CostEffectiveness.Value)),
                            Max = string.Format("{0:G3}", i.Max(d => d.CostEffectiveness.Value)),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Cost Effectiveness";
                return View("Heatmap");
            }
        }


        public ActionResult RevenuePerCapita(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "zip")
                {
                    var entities = GetZipsInBounds(context).ToList();

                    var data = GetZipCodeData(context, industryId)
                        .Where(i => i.RevenuePerCapita > 0)
                        .Select(i => new { i.RevenuePerCapita, i.ZipCode })
                        .ToList()
                        .NTile(i => i.RevenuePerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.RevenuePerCapita.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.RevenuePerCapita.Value))),
                            Items = i.Where(d => entities.Contains(d.ZipCode.Id)).Select(d => d.ZipCode.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.RevenuePerCapita > 0)
                        .Select(i => new { i.RevenuePerCapita, i.County })
                        .ToList()
                        .NTile(i => i.RevenuePerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.RevenuePerCapita.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.RevenuePerCapita.Value))),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.RevenuePerCapita > 0)
                        .Select(i => new { i.RevenuePerCapita, i.State })
                        .ToList()
                        .NTile(i => i.RevenuePerCapita, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.RevenuePerCapita.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.RevenuePerCapita.Value))),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Revenue Per Capita";
                return View("Heatmap");
            }
        }

        public ActionResult TotalRevenue(int bands, int industryId, string levelOfDetail)
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };
            using (var context = ContextFactory.SizeUpContext)
            {
                if (levelOfDetail == "zip")
                {
                    var entities = GetZipsInBounds(context).ToList();

                    var data = GetZipCodeData(context, industryId)
                        .Where(i => i.TotalRevenue > 0)
                        .Select(i => new { i.TotalRevenue, i.ZipCode })
                        .ToList()
                        .NTile(i => i.TotalRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.TotalRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.TotalRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.ZipCode.Id)).Select(d => d.ZipCode.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "Zip Code";
                }
                else if (levelOfDetail == "county")
                {
                    var entities = GetCountiesInBounds(context).ToList();

                    var data = GetCountyData(context, industryId)
                        .Where(i => i.TotalRevenue > 0)
                        .Select(i => new { i.TotalRevenue, i.County })
                        .ToList()
                        .NTile(i => i.TotalRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.TotalRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.TotalRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.County.Id)).Select(d => d.County.Name + " County, " + d.County.State.Abbreviation).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "County";
                }
                else if (levelOfDetail == "state")
                {
                    var entities = GetStatesInBounds(context).ToList();

                    var data = GetStateData(context, industryId)
                        .Where(i => i.TotalRevenue > 0)
                        .Select(i => new { i.TotalRevenue, i.State })
                        .ToList()
                        .NTile(i => i.TotalRevenue, bands)
                        .ToList()
                        .Select(i => new Band
                        {
                            Min = string.Format("${0}", Format(i.Min(d => d.TotalRevenue.Value))),
                            Max = string.Format("${0}", Format(i.Max(d => d.TotalRevenue.Value))),
                            Items = i.Where(d => entities.Contains(d.State.Id)).Select(d => d.State.Name).ToList()
                        })
                        .Reverse()
                        .ToList();

                    ViewBag.Bands = FormatBands(data);
                    ViewBag.LevelOfDetail = "State";
                }
                ViewBag.BoundingEntity = GetBoundingEntityName(context);
                ViewBag.Attribute = "Total Revenue";
                return View("Heatmap");
            }
        }
    }
}
