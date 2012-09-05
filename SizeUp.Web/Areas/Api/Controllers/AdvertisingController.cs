using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using System.Data.Spatial;
using Microsoft.SqlServer.Types;
using SizeUp.Web;
using SizeUp.Web.Areas.Api.Models.Advertising;
using SizeUp.Core;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AdvertisingController : Controller
    {
        //
        // GET: /Api/Advertising/

        private Range ParseQueryString(string index)
        {
            Range v = null;
            int?[] ar = QueryString.IntValues(index);

            if (ar != null)
            {
                v = new Models.Advertising.Range();
                v.Min = ar[0];
                v.Max = ar[1];
            }
            return v;
        }


        public ActionResult Advertising(int industryId, long placeId, int page = 1, int itemCount = 20)
        {

            if (!User.Identity.IsAuthenticated)
            {
                page = 1;
                itemCount = 3;
            }

            int? distance = QueryString.IntValue("distance");

            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range totalEmployees = ParseQueryString("totalEmployees");
            Range revenuePerCapita = ParseQueryString("revenuePerCapita");

            Range householdIncome = ParseQueryString("householdIncome");
            Range householdExpenditures = ParseQueryString("householdExpenditures");
            Range medianAge = ParseQueryString("medianAge");
            int? bachelorOrHigher = QueryString.IntValue("bachelorsDegreeOrHigher");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollar = QueryString.IntValue("whiteCollarWorkers");
            string sort = QueryString.StringValue("sort");
            string sortAttribute = QueryString.StringValue("sortAttribute");


            using (var context = ContextFactory.SizeUpContext)
            {
                var placePolys = context.CityCountyMappings.Where(i => i.Id == placeId)
                   .Select(i => new
                   {
                       City = i.City.CityGeographies.Where(g => g.CityId == i.CityId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                       County = i.County.CountyGeographies.Where(g => g.CountyId == i.CountyId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()
                   })
                   .FirstOrDefault();

                    var geo = SqlGeography.Parse(placePolys.City.Intersection(placePolys.County).AsText());
                    var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                    geom = geom.STCentroid();
                    geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                    var center = new
                    {
                        lat = (double)geo.STPointN(1).Lat,
                        lng = (double)geo.STPointN(1).Long
                    };
              


                var data = ZipCodes.GetWithDistance(context, center.lat, center.lng)
                    .Select(i=> new {
                        IndustryData = i.Entity.IndustryDataByZips.Where(id=>id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                        Demographics = i.Entity.DemographicsByZips.Where(d=> d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault(),
                        ZipCode  = i
                    })
                
                /*
                var data = context.DemographicsByZips
                    .Join(context.IndustryDataByZips, i=> i.ZipCodeId , o=>o.ZipCodeId, (i,o)=> new { Demographics = i, IndustryData = o})
                    .Join(zips, i => i.IndustryData.ZipCodeId, o => o.Entity.Id, (i, o) => new { ZipCode = o, IndustryData = i.IndustryData, Demographics = i.Demographics })
                    .Where(i=>i.Demographics.Year == TimeSlice.Year &&
                            i.Demographics.Quarter == TimeSlice.Quarter &&
                            i.IndustryData.Year == TimeSlice.Year &&
                            i.IndustryData.Quarter == TimeSlice.Quarter &&
                            i.IndustryData.IndustryId == industryId)
                */



                //var data = ZipCodes.GetWithDistance(context, center.lat, center.lng)
                //    .Join(IndustryData.GetZipCodes(context, industryId), i => i.Entity.Id, o => o.ZipCodeId, (i, o) => new { ZipCode = i, IndustryData = o })
               //     .Join(DemographicsData.GetZipCodes(context), i => i.IndustryData.ZipCodeId, o => o.ZipCodeId, (i, o) => new { ZipCode = i.ZipCode, IndustryData = i.IndustryData, Demographics = o })
                    .Select(i => new
                    {
                        ZipCode = i.ZipCode,
                        ZipCodeId = i.IndustryData.ZipCode.Id,
                        Name = i.IndustryData.ZipCode.Name,
                        Center = i.IndustryData.ZipCode.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        AverageRevenue = (long)i.IndustryData.AverageRevenue,
                        TotalRevenue = (long)i.IndustryData.TotalRevenue,
                        TotalEmployees = (long)i.IndustryData.TotalEmployees,
                        RevenuePerCapita = (long)i.IndustryData.RevenuePerCapita,
                        BachelorsDegreeOrHigher = (double)i.Demographics.BachelorsOrHigherPercentage,
                        HighSchoolOrHigher = (double)i.Demographics.HighSchoolOrHigherPercentage,
                        MedianAge = (int)i.Demographics.MedianAge,
                        HouseholdIncome = (long)i.Demographics.MedianHouseholdIncome,
                        HouseholdExpenditures = (long)i.Demographics.AverageHouseholdExpenditure,
                        WhiteCollarWorkers = (double)i.Demographics.WhiteCollarWorkersPercentage,
                        TotalPopulation = (long)i.Demographics.TotalPopulation
                    });
                   



                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees <= totalEmployees.Max);
                    }
                }

                if (revenuePerCapita != null)
                {
                    if (revenuePerCapita.Min.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita >= revenuePerCapita.Min);
                    }
                    if (revenuePerCapita.Max.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita <= revenuePerCapita.Max);
                    }
                }

                if (householdIncome != null)
                {
                    if (householdIncome.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome >= householdIncome.Min);
                    }
                    if (householdIncome.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome <= householdIncome.Max);
                    }
                }

                if (householdExpenditures != null)
                {
                    if (householdExpenditures.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures >= householdExpenditures.Min);
                    }
                    if (householdExpenditures.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures <= householdExpenditures.Max);
                    }
                }

                if (medianAge != null)
                {
                    if (medianAge.Min.HasValue)
                    {
                        data = data.Where(i => i.MedianAge >= medianAge.Min);
                    }
                    if (medianAge.Max.HasValue)
                    {
                        data = data.Where(i => i.MedianAge <= medianAge.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    var v = bachelorOrHigher / 100.0d;
                    data = data.Where(i => i.BachelorsDegreeOrHigher >= v);
                }

                if (highSchoolOrHigher != null)
                {
                    var v = highSchoolOrHigher / 100.0d;
                    data = data.Where(i => i.HighSchoolOrHigher >= v);
                }

                if (whiteCollar != null)
                {
                    var v = whiteCollar / 100.0d;
                    data = data.Where(i => i.WhiteCollarWorkers >= v);
                }



                if (distance != null)
                {
                    data = data.Where(i => i.ZipCode.Distance < distance.Value);
                }



                var results = data
                   .Select(i=> new Advertising()
                   {
                       ZipCodeId = i.ZipCode.Entity.Id,
                       Name = i.ZipCode.Entity.Name,
                       Lat = (double)i.Center.Lat,
                       Long = (double)i.Center.Lng,
                       City = context.ZipCodePlaceMappings.Where(o=>o.ZipCodeId == i.ZipCode.Entity.Id)
                       .Select(o=> new Models.City.City()
                       {
                           Id = o.CityCountyMapping.City.Id,
                           Name = o.CityCountyMapping.City.Name,
                           SEOKey = o.CityCountyMapping.City.SEOKey,
                           State = o.CityCountyMapping.City.State.Abbreviation
                       }).FirstOrDefault(),
                       County = context.ZipCodePlaceMappings.Where(o => o.ZipCodeId == i.ZipCode.Entity.Id)
                       .Select(o => new Models.County.County()
                       {
                           Id = o.CityCountyMapping.County.Id,
                           Name = o.CityCountyMapping.County.Name,
                           SEOKey = o.CityCountyMapping.County.SEOKey,
                           State = o.CityCountyMapping.County.State.Abbreviation
                       }).FirstOrDefault(),
                       State = context.ZipCodePlaceMappings.Where(o => o.ZipCodeId == i.ZipCode.Entity.Id)
                       .Select(o => new Models.State.State()
                       {
                           Id = o.CityCountyMapping.County.State.Id,
                           Abbreviation = o.CityCountyMapping.County.State.Abbreviation,
                           Name = o.CityCountyMapping.County.State.Name,
                           SEOKey = o.CityCountyMapping.County.State.SEOKey
                       }).FirstOrDefault(),
                       AverageRevenue = i.AverageRevenue,
                       TotalRevenue = i.TotalRevenue,
                       TotalEmployees = i.TotalEmployees,
                       RevenuePerCapita = i.RevenuePerCapita,
                       BachelorsDegreeOrHigher = i.BachelorsDegreeOrHigher,
                       HighSchoolOrHigher = i.HighSchoolOrHigher,
                       MedianAge = i.MedianAge,
                       HouseholdIncome = i.HouseholdIncome,
                       HouseholdExpenditures = i.HouseholdExpenditures,
                       WhiteCollarWorkers = i.WhiteCollarWorkers,
                       TotalPopulation = i.TotalPopulation
                   });

                switch (sortAttribute)
                {
                    case "name":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.Name);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.Name);
                        }
                        break;
                    case "totalRevenue":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.TotalRevenue);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.TotalRevenue);
                        }
                        break;
                    case "averageRevenue":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.AverageRevenue);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.AverageRevenue);
                        }
                        break;
                    case "revenuePerCapita":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.RevenuePerCapita);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.RevenuePerCapita);
                        }
                        break;
                    case "householdIncome":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.HouseholdIncome);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.HouseholdIncome);
                        }
                        break;

                    case "totalPopulation":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.TotalPopulation);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.TotalPopulation);
                        }
                        break;

                    case "whiteCollarWorkers":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.WhiteCollarWorkers);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.WhiteCollarWorkers);
                        }
                        break;

                    case "totalEmployees":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.TotalEmployees);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.TotalEmployees);
                        }
                        break;
                    case "householdExpenditures":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.HouseholdExpenditures);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.HouseholdExpenditures);
                        }
                        break;
                    case "medianAge":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.MedianAge);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.MedianAge);
                        }
                        break;
                    case "bachelorsDegreeOrHigher":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.BachelorsDegreeOrHigher);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.BachelorsDegreeOrHigher);
                        }
                        break;
                    case "highSchoolOrHigher":
                        if (sort == "desc")
                        {
                            results = results.OrderByDescending(i => i.HighSchoolOrHigher);
                        }
                        else
                        {
                            results = results.OrderBy(i => i.HighSchoolOrHigher);
                        }
                        break;

                    default:
                        results = results.OrderBy(i => i.Name);
                        break;

                }

                var output = new
                {
                    Total = results.Count(),
                    Items = results
                        .Skip((page - 1) * itemCount)
                        .Take(itemCount)
                        .ToList()
                };

                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult MinimumDistance(int industryId, long placeId, int itemCount)
        {
            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range totalEmployees = ParseQueryString("totalEmployees");
            Range revenuePerCapita = ParseQueryString("revenuePerCapita");

            Range householdIncome = ParseQueryString("householdIncome");
            Range householdExpenditures = ParseQueryString("householdExpenditures");
            Range medianAge = ParseQueryString("medianAge");
            int? bachelorOrHigher = QueryString.IntValue("bachelorsDegreeOrHigher");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollar = QueryString.IntValue("whiteCollarWorkers");


            using (var context = ContextFactory.SizeUpContext)
            {
                var placePolys = context.CityCountyMappings.Where(i => i.Id == placeId)
                   .Select(i => new
                   {
                       City = i.City.CityGeographies.Where(g => g.CityId == i.CityId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                       County = i.County.CountyGeographies.Where(g => g.CountyId == i.CountyId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()
                   })
                   .FirstOrDefault();

                var geo = SqlGeography.Parse(placePolys.City.Intersection(placePolys.County).AsText());
                var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                geom = geom.STCentroid();
                geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                var center = new
                {
                    lat = (double)geo.STPointN(1).Lat,
                    lng = (double)geo.STPointN(1).Long
                };


                var data = ZipCodes.GetWithDistance(context, center.lat, center.lng)
                    .Select(i => new
                    {
                        IndustryData = i.Entity.IndustryDataByZips.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                        Demographics = i.Entity.DemographicsByZips.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault(),
                        ZipCode = i
                    })
                    .Select(i => new
                    {
                        ZipCode = i.ZipCode,
                        ZipCodeId = i.IndustryData.ZipCode.Id,
                        Name = i.IndustryData.ZipCode.Name,
                        Center = i.IndustryData.ZipCode.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        AverageRevenue = (long)i.IndustryData.AverageRevenue,
                        TotalRevenue = (long)i.IndustryData.TotalRevenue,
                        TotalEmployees = (long)i.IndustryData.TotalEmployees,
                        RevenuePerCapita = (long)i.IndustryData.RevenuePerCapita,
                        BachelorsDegreeOrHigher = (double)i.Demographics.BachelorsOrHigherPercentage,
                        HighSchoolOrHigher = (double)i.Demographics.HighSchoolOrHigherPercentage,
                        MedianAge = (int)i.Demographics.MedianAge,
                        HouseholdIncome = (long)i.Demographics.MedianHouseholdIncome,
                        HouseholdExpenditures = (long)i.Demographics.AverageHouseholdExpenditure,
                        WhiteCollarWorkers = (double)i.Demographics.WhiteCollarWorkersPercentage,
                        TotalPopulation = (long)i.Demographics.TotalPopulation
                    });
                   


                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees <= totalEmployees.Max);
                    }
                }

                if (revenuePerCapita != null)
                {
                    if (revenuePerCapita.Min.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita >= revenuePerCapita.Min);
                    }
                    if (revenuePerCapita.Max.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita <= revenuePerCapita.Max);
                    }
                }

                if (householdIncome != null)
                {
                    if (householdIncome.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome >= householdIncome.Min);
                    }
                    if (householdIncome.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome <= householdIncome.Max);
                    }
                }

                if (householdExpenditures != null)
                {
                    if (householdExpenditures.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures >= householdExpenditures.Min);
                    }
                    if (householdExpenditures.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures <= householdExpenditures.Max);
                    }
                }

                if (medianAge != null)
                {
                    if (medianAge.Min.HasValue)
                    {
                        data = data.Where(i => i.MedianAge >= medianAge.Min);
                    }
                    if (medianAge.Max.HasValue)
                    {
                        data = data.Where(i => i.MedianAge <= medianAge.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    var v = bachelorOrHigher / 100.0d;
                    data = data.Where(i => i.BachelorsDegreeOrHigher >= v);
                }

                if (highSchoolOrHigher != null)
                {
                    var v = highSchoolOrHigher / 100.0d;
                    data = data.Where(i => i.HighSchoolOrHigher >= v);
                }

                if (whiteCollar != null)
                {
                    var v = whiteCollar / 100.0d;
                    data = data.Where(i => i.WhiteCollarWorkers >= v);
                }

                var results = data.Select(i => new
                {
                    i.ZipCode.Distance
                });

                results = results.OrderBy(i => i.Distance);

                var distance = results.Skip(itemCount - 1).FirstOrDefault();
                int? miles = null;
                if (distance != null)
                {
                    miles = (int)System.Math.Ceiling(distance.Distance);
                }


                return Json(miles, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bands(int industryId, long placeId, int bands)
        {
            int? distance = QueryString.IntValue("distance");

            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range totalEmployees = ParseQueryString("totalEmployees");
            Range revenuePerCapita = ParseQueryString("revenuePerCapita");

            Range householdIncome = ParseQueryString("householdIncome");
            Range householdExpenditures = ParseQueryString("householdExpenditures");
            Range medianAge = ParseQueryString("medianAge");
            int? bachelorOrHigher = QueryString.IntValue("bachelorsDegreeOrHigher");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollar = QueryString.IntValue("whiteCollarWorkers");
            string sort = QueryString.StringValue("sort");
            string sortAttribute = QueryString.StringValue("sortAttribute");
            string attribute = QueryString.StringValue("attribute");

            using (var context = ContextFactory.SizeUpContext)
            {
                var placePolys = context.CityCountyMappings.Where(i => i.Id == placeId)
                   .Select(i => new
                   {
                       City = i.City.CityGeographies.Where(g => g.CityId == i.CityId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                       County = i.County.CountyGeographies.Where(g => g.CountyId == i.CountyId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()
                   })
                   .FirstOrDefault();

                var geo = SqlGeography.Parse(placePolys.City.Intersection(placePolys.County).AsText());
                var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                geom = geom.STCentroid();
                geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                var center = new
                {
                    lat = (double)geo.STPointN(1).Lat,
                    lng = (double)geo.STPointN(1).Long
                };



                var data = ZipCodes.GetWithDistance(context, center.lat, center.lng)
                      .Select(i => new
                      {
                          IndustryData = i.Entity.IndustryDataByZips.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                          Demographics = i.Entity.DemographicsByZips.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault(),
                          ZipCode = i
                      })
                    .Select(i => new
                    {
                        ZipCode = i.ZipCode,
                        ZipCodeId = i.IndustryData.ZipCode.Id,
                        Name = i.IndustryData.ZipCode.Name,
                        Center = i.IndustryData.ZipCode.ZipCodeGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        AverageRevenue = (long)i.IndustryData.AverageRevenue,
                        TotalRevenue = (long)i.IndustryData.TotalRevenue,
                        TotalEmployees = (long)i.IndustryData.TotalEmployees,
                        RevenuePerCapita = (long)i.IndustryData.RevenuePerCapita,
                        BachelorsDegreeOrHigher = (double)i.Demographics.BachelorsOrHigherPercentage,
                        HighSchoolOrHigher = (double)i.Demographics.HighSchoolOrHigherPercentage,
                        MedianAge = (int)i.Demographics.MedianAge,
                        HouseholdIncome = (long)i.Demographics.MedianHouseholdIncome,
                        HouseholdExpenditures = (long)i.Demographics.AverageHouseholdExpenditure,
                        WhiteCollarWorkers = (double)i.Demographics.WhiteCollarWorkersPercentage,
                        TotalPopulation = (long)i.Demographics.TotalPopulation
                    });



                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        data = data.Where(i => i.TotalEmployees <= totalEmployees.Max);
                    }
                }

                if (revenuePerCapita != null)
                {
                    if (revenuePerCapita.Min.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita >= revenuePerCapita.Min);
                    }
                    if (revenuePerCapita.Max.HasValue)
                    {
                        data = data.Where(i => i.RevenuePerCapita <= revenuePerCapita.Max);
                    }
                }

                if (householdIncome != null)
                {
                    if (householdIncome.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome >= householdIncome.Min);
                    }
                    if (householdIncome.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdIncome <= householdIncome.Max);
                    }
                }

                if (householdExpenditures != null)
                {
                    if (householdExpenditures.Min.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures >= householdExpenditures.Min);
                    }
                    if (householdExpenditures.Max.HasValue)
                    {
                        data = data.Where(i => i.HouseholdExpenditures <= householdExpenditures.Max);
                    }
                }

                if (medianAge != null)
                {
                    if (medianAge.Min.HasValue)
                    {
                        data = data.Where(i => i.MedianAge >= medianAge.Min);
                    }
                    if (medianAge.Max.HasValue)
                    {
                        data = data.Where(i => i.MedianAge <= medianAge.Max);
                    }
                }

                if (bachelorOrHigher != null)
                {
                    var v = bachelorOrHigher / 100.0d;
                    data = data.Where(i => i.BachelorsDegreeOrHigher >= v);
                }

                if (highSchoolOrHigher != null)
                {
                    var v = highSchoolOrHigher / 100.0d;
                    data = data.Where(i => i.HighSchoolOrHigher >= v);
                }

                if (whiteCollar != null)
                {
                    var v = whiteCollar / 100.0d;
                    data = data.Where(i => i.WhiteCollarWorkers >= v);
                }

                if (distance != null)
                {
                    data = data.Where(i => i.ZipCode.Distance < distance.Value);
                }

                var results = data
                   .Select(i => new Advertising(){
                       AverageRevenue = i.AverageRevenue,
                       TotalRevenue = i.TotalRevenue,
                       TotalEmployees = i.TotalEmployees,
                       RevenuePerCapita = i.RevenuePerCapita,
                       BachelorsDegreeOrHigher = i.BachelorsDegreeOrHigher,
                       HighSchoolOrHigher = i.HighSchoolOrHigher,
                       MedianAge = i.MedianAge,
                       HouseholdIncome = i.HouseholdIncome,
                       HouseholdExpenditures = i.HouseholdExpenditures,
                       WhiteCollarWorkers = i.WhiteCollarWorkers,
                       TotalPopulation = i.TotalPopulation
                   });

                List<object> output = null;
                if (attribute == "totalRevenue")
                {
                    output = results
                        .Select(i => i.TotalRevenue ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "averageRevenue")
                {
                    output = results
                        .Select(i => i.AverageRevenue ??0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "revenuePerCapita")
                {
                    output = results
                        .Select(i => i.RevenuePerCapita ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "householdIncome")
                {
                    output = results
                        .Select(i => i.HouseholdIncome ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "totalPopulation")
                {
                    output = results
                        .Select(i => i.TotalPopulation ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "whiteCollarWorkers")
                {
                    output = results
                        .Select(i => i.WhiteCollarWorkers ?? 0 )
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<double>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<double> old = null;
                    foreach (Models.Advertising.Band<double> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 0.001;
                        }
                        old = band;
                    }
                }
                else if (attribute == "totalEmployees")
                {
                    output = results
                        .Select(i => i.TotalEmployees ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "householdExpenditures")
                {
                    output = results
                        .Select(i => i.HouseholdExpenditures ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "medianAge")
                {
                    output = results
                        .Select(i => i.MedianAge ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<long>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<long> old = null;
                    foreach (Models.Advertising.Band<long> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 1;
                        }
                        old = band;
                    }
                }
                else if (attribute == "bachelorsDegreeOrHigher")
                {
                    output = results
                       .Select(i => i.BachelorsDegreeOrHigher ?? 0)
                       .ToList()
                       .NTile(i => i, bands)
                       .Select(b => new Models.Advertising.Band<double>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                       .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<double> old = null;
                    foreach (Models.Advertising.Band<double> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 0.001;
                        }
                        old = band;
                    }
                }
                else if (attribute == "highSchoolOrHigher")
                {
                    output = results
                        .Select(i => i.HighSchoolOrHigher ?? 0)
                        .ToList()
                        .NTile(i => i, bands)
                        .Select(b => new Models.Advertising.Band<double>() { Min = b.Min(i => i), Max = b.Max(i => i) })
                        .Cast<object>()
                        .ToList();

                    Models.Advertising.Band<double> old = null;
                    foreach (Models.Advertising.Band<double> band in output)
                    {
                        if (old != null)
                        {
                            old.Max = band.Min - 0.001;
                        }
                        old = band;
                    }
                }
                output.Reverse();
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
