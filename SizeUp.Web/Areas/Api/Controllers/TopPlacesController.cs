﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Web.Areas.Api.Models.TopPlaces;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TopPlacesController : Controller
    {
        //
        // GET: /Api/TopPlaces/
        private Range ParseQueryString(string index)
        {
            Range v = null;
            int?[] ar = QueryString.IntValues(index);

            if (ar != null)
            {
                v = new Models.TopPlaces.Range();
                v.Min = ar[0];
                v.Max = ar[1];
            }
            return v;
        }


        public ActionResult City(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");
           

            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Cities.Select(i => new
                {
                    City = i,
                    IndustryData = i.IndustryDataByCities.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.City.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.City.StateId == stateId.Value);
                }


                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }



                var raw = entities.Select(i => new
                    {
                        City = new Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            Centroid = i.City.CityGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        },
                        County = i.City.CityCountyMappings.Select(c => new Models.County.County()
                        {
                            Id = c.County.Id,
                            Name = c.County.Name,
                            SEOKey = c.County.SEOKey,
                            Centroid = c.County.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        }).FirstOrDefault(),
                        State = new Models.State.State()
                        {
                            Id = i.City.State.Id,
                            Name = i.City.State.Name,
                            Abbreviation = i.City.State.Abbreviation,
                            SEOKey = i.City.State.SEOKey,
                            Centroid = i.City.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData,
                        Demographics = i.Demographics
                    });



                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }


                var outData = data
                    .Take(itemCount)
                    .ToList();


                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }
      

        public ActionResult County(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");
           


            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Counties.Select(i => new
                {
                    County = i,
                    IndustryData = i.IndustryDataByCounties.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCounties.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.County.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.County.StateId == stateId.Value);
                }


                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }



                var raw = entities
                    .Select(i => new
                    {
                        County = new Models.County.County(){
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey,
                            Centroid = i.County.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        },
                        State = new Models.State.State()
                        {
                            Id = i.County.State.Id,
                            Name = i.County.State.Name,
                            Abbreviation = i.County.State.Abbreviation,
                            SEOKey = i.County.State.SEOKey,
                            Centroid = i.County.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData,
                        Demographics = i.Demographics
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }


                var outData = data
                    .Take(itemCount)
                    .ToList();


                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Metro(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");
           


            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.Metroes.Select(i => new
                {
                    Metro = i,
                    IndustryData = i.IndustryDataByMetroes.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByMetroes.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                

                if (regionId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c=>c.State.DivisionId == regionId.Value));
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c=>c.StateId == stateId.Value));
                }

                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }


                var raw = entities
                    .Select(i => new
                    {
                        Metro = new Models.Metro.Metro()
                        {
                            Id = i.Metro.Id,
                            Name = i.Metro.Name,
                            SEOKey = i.Metro.SEOKey,
                            Centroid = i.Metro.MetroGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }


                var outData = data
                    .Take(itemCount)
                    .ToList();


                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult State(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");
           


            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.States.Select(i => new
                {
                    State = i,
                    IndustryData = i.IndustryDataByStates.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByStates.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });



                if (regionId != null)
                {
                    entities = entities.Where(i => i.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.State.Id == stateId.Value);
                }

                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }


                var raw = entities
                    .Select(i => new
                    {
                        State = new Models.State.State()
                        {
                            Id = i.State.Id,
                            Name = i.State.Name,
                            Abbreviation = i.State.Abbreviation,
                            SEOKey = i.State.SEOKey,
                            Centroid = i.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i=>i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }


                var outData = data
                    .Take(itemCount)
                    .ToList();
                    

                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }


        ///BANDS
        ///

        public ActionResult CityBands(int bands, int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");


            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Cities.Select(i => new
                {
                    City = i,
                    IndustryData = i.IndustryDataByCities.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.City.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.City.StateId == stateId.Value);
                }


                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }



                var raw = entities.Select(i => new
                {
                    IndustryData = i.IndustryData,
                    Demographics = i.Demographics
                });



                List<object> output = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => i.IndustryData.TotalRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => i.IndustryData.AverageRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => i.IndustryData.TotalEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => i.IndustryData.AverageEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => i.IndustryData.EmployeesPerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<double?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<double?> old = null;
                    foreach (Models.Shared.Band<double?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                }


                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CountyBands(int bands, int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");



            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Counties.Select(i => new
                {
                    County = i,
                    IndustryData = i.IndustryDataByCounties.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCounties.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.County.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.County.StateId == stateId.Value);
                }


                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }



                var raw = entities.Select(i => new
                {
                    IndustryData = i.IndustryData,
                    Demographics = i.Demographics
                });



                List<object> output = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => i.IndustryData.TotalRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => i.IndustryData.AverageRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => i.IndustryData.TotalEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => i.IndustryData.AverageEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => i.IndustryData.EmployeesPerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<double?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<double?> old = null;
                    foreach (Models.Shared.Band<double?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                }



                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MetroBands(int bands, int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");



            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.Metroes.Select(i => new
                {
                    Metro = i,
                    IndustryData = i.IndustryDataByMetroes.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByMetroes.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });



                if (regionId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c => c.State.DivisionId == regionId.Value));
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c => c.StateId == stateId.Value));
                }

                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }


                var raw = entities.Select(i => new
                {
                    IndustryData = i.IndustryData,
                    Demographics = i.Demographics
                });



                List<object> output = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => i.IndustryData.TotalRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => i.IndustryData.AverageRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => i.IndustryData.TotalEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => i.IndustryData.AverageEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => i.IndustryData.EmployeesPerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<double?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<double?> old = null;
                    foreach (Models.Shared.Band<double?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                }



                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StateBands(int bands, int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range population = ParseQueryString("population");

            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? blueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            int? airportsNearby = QueryString.IntValue("airportsNearby");
            int? youngEducated = QueryString.IntValue("youngEducated");
            int? universitiesNearby = QueryString.IntValue("universitiesNearby");
            int? commuteTime = QueryString.IntValue("commuteTime");



            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.States.Select(i => new
                {
                    State = i,
                    IndustryData = i.IndustryDataByStates.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByStates.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });



                if (regionId != null)
                {
                    entities = entities.Where(i => i.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.State.Id == stateId.Value);
                }

                if (population != null)
                {
                    if (population.Min.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation >= population.Min);
                    }
                    if (population.Max.HasValue)
                    {
                        entities = entities.Where(i => i.Demographics.TotalPopulation <= population.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.BachelorsOrHigherPercentage >= (float)bachelorOrHigher / 100);
                }
                if (blueCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.BlueCollarWorkersPercentage >= (float)blueCollarWorkers / 100);
                }
                if (highSchoolOrHigher != null)
                {
                    entities = entities.Where(i => i.Demographics.HighSchoolOrHigherPercentage >= (float)highSchoolOrHigher / 100);
                }
                if (whiteCollarWorkers != null)
                {
                    entities = entities.Where(i => i.Demographics.WhiteCollarWorkersPercentage >= (float)whiteCollarWorkers / 100);
                }
                if (airportsNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.AirPortsWithin50Miles >= airportsNearby);
                }
                if (youngEducated != null)
                {
                    entities = entities.Where(i => i.Demographics.YoungEducatedPercentage >= (float)youngEducated / 100);
                }
                if (universitiesNearby != null)
                {
                    entities = entities.Where(i => i.Demographics.UniversitiesWithinHalfMile >= universitiesNearby);
                }
                if (commuteTime != null)
                {
                    entities = entities.Where(i => i.Demographics.CommuteTime >= commuteTime);
                }


                var raw = entities.Select(i => new
                {
                    IndustryData = i.IndustryData,
                    Demographics = i.Demographics
                });



                List<object> output = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => i.IndustryData.TotalRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => i.IndustryData.AverageRevenue)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => i.IndustryData.TotalEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => i.IndustryData.AverageEmployees)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => i.IndustryData.EmployeesPerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<double?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<double?> old = null;
                    foreach (Models.Shared.Band<double?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                    output.Reverse();
                }
                else if (attribute.Equals("underservedMarkets", StringComparison.CurrentCultureIgnoreCase))
                {
                    output = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderBy(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => i.IndustryData.RevenuePerCapita)
                        .Take(itemCount)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Shared.Band<long?>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Shared.Band<long?> old = null;
                    foreach (Models.Shared.Band<long?> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min;
                        }
                        old = band;
                    }
                }



                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
