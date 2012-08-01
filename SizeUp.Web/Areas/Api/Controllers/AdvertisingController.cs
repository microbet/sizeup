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
            int? bachelorOrHigher = QueryString.IntValue("bachelorOrHigher");
            int? highSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            int? whiteCollar = QueryString.IntValue("whiteCollar");
            string sort = QueryString.StringValue("sort");
            string sortAttribute = QueryString.StringValue("sortAttribute");
            

            using (var context = ContextFactory.SizeUpContext)
            {
                var center = Locations.Get(context, placeId)
                    .Select(i=>i.City.CityGeographies.Where(g=>g.GeographyClass.Name == "Calculation").Select(g=>new { lat = g.Geography.CenterLat, lng = g.Geography.CenterLong}).FirstOrDefault())
                    .FirstOrDefault();

                var data = IndustryData.GetZipCodes(context, industryId)
                    .Select(i=> new {
                            //City = i.ZipCode.ZipCodeCityMappings.Where(c => i.ZipCode.Name.StartsWith(c.City.Name)).Select(c => c.City).FirstOrDefault() ?? i.ZipCode.ZipCodeCityMappings.Select(c => c.City).FirstOrDefault(),
                            ZipCode = i.ZipCode,                          
                            AverageRevenue = (long)i.AverageRevenue,
                            TotalRevenue = (long)i.TotalRevenue,
                            TotalEmployees = (long)i.TotalEmployees,
                            RevenuePerCapita = (long)i.RevenuePerCapita,
                            BachelorsDegreeOrHigher = (double)i.BachelorsOrHigher,
                            HighSchoolOrHigher = (double)i.HighSchoolOrHigher,
                            MedianAge = (int)i.MedianAge,
                            HouseholdIncome = (long)i.MedianHouseholdIncome,
                            HouseholdExpendatures = (long)i.AverageHouseholdExpenditure,
                            WhiteCollarWorkers = (double)i.WhiteCollarWorkers,
                            TotalPopulation = (long)i.TotalPopulation
                        })
                        .Select(i => new Advertising()
                        {
                            ZipCodeId = i.ZipCode.Id,
                           /* City = new Models.City.City(){
                                Id = i.City.Id,
                                Name = i.City.Name,
                                SEOKey = i.City.SEOKey,
                                State = i.City.State.Abbreviation
                            },
                            County =  i.City.CityCountyMappings.Select(c=> new Models.County.County(){
                                Id = c.County.Id,
                                Name = c.County.Name,
                                SEOKey = c.County.SEOKey,
                                State = c.County.State.Abbreviation
                            }).FirstOrDefault(),
                            State = new Models.State.State(){
                                Id = i.City.State.Id,
                                Name = i.City.State.Name,
                                Abbreviation = i.City.State.Abbreviation,
                                SEOKey = i.City.State.SEOKey
                            },*/
                            Name = i.ZipCode.Name,                          
                            AverageRevenue = i.AverageRevenue,
                            TotalRevenue = i.TotalRevenue,
                            TotalEmployees = i.TotalEmployees,
                            RevenuePerCapita = i.RevenuePerCapita,
                            BachelorsDegreeOrHigher = i.BachelorsDegreeOrHigher,
                            HighSchoolOrHigher = i.HighSchoolOrHigher,
                            MedianAge = i.MedianAge,
                            HouseholdIncome = i.HouseholdIncome,
                            HouseholdExpenditures = i.HouseholdExpendatures,
                            WhiteCollarWorkers = i.WhiteCollarWorkers,
                            TotalPopulation = i.TotalPopulation
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
                    var v  =  bachelorOrHigher / 100.0d;
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
                    var zips = ZipCodes.GetWithin(context, center.lat.Value, center.lng.Value, distance.Value);
                    data = data.Join(zips, i => i.ZipCodeId, i => i.Id, (i, o) => i);
                }



                switch (sortAttribute)
                {
                    case "Name":
                        if (sort == "desc")
                        {
                            data = data.OrderByDescending(i => i.Name);
                        }
                        else
                        {
                            data = data.OrderBy(i => i.Name);
                        }
                        break;
                    case "TotalRevenue":
                        if (sort == "desc")
                        {
                            data = data.OrderByDescending(i => i.TotalRevenue);
                        }
                        else
                        {
                            data = data.OrderBy(i => i.TotalRevenue);
                        }
                        break;
                    case "AverageRevenue":
                        if (sort == "desc")
                        {
                            data = data.OrderByDescending(i => i.AverageRevenue);
                        }
                        else
                        {
                            data = data.OrderBy(i => i.AverageRevenue);
                        }
                        break;
                    case "RevenuePerCapita":
                        if (sort == "desc")
                        {
                            data = data.OrderByDescending(i => i.RevenuePerCapita);
                        }
                        else
                        {
                            data = data.OrderBy(i => i.RevenuePerCapita);
                        }
                        break;
                    case "HouseholdIncome":
                        if (sort == "desc")
                        {
                            data = data.OrderByDescending(i => i.HouseholdIncome);
                        }
                        else
                        {
                            data = data.OrderBy(i => i.HouseholdIncome);
                        }
                        break;

                    default:
                        data = data.OrderBy(i => i.Name);
                        break;

                }

               



                var output = new
                {
                    Total = data.Count(),
                    Items = data
                        .Skip((page - 1) * itemCount)
                        .Take(itemCount)
                        .ToList()
                };
                 
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
