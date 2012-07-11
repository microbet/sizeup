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


        public ActionResult Advertising(int industryId, long cityId, int page = 1, int itemCount = 20)
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

            

            using (var context = ContextFactory.SizeUpContext)
            {

                var zips = context.ZipCodes.AsQueryable();
                if (distance != null)
                {

                    var cityCenter = context.CityGeographies
                        .Where(i => i.CityId == cityId && i.GeographyClass.Name == "Calculation")
                        .Select(i => DbGeography.FromBinary(DbGeometry.FromBinary(i.Geography.GeographyPolygon.AsBinary()).Centroid.AsBinary(), 4326)).FirstOrDefault();

                    var zipGeos = context.ZipCodeGeographies.Where(i => i.GeographyClass.Name == "Calculation" && i.Geography.GeographyPolygon.Distance(cityCenter) < distance * 1609.344);
                    zips = zips.Join(zipGeos, i => i.Id, i => i.ZipCodeId, (i, o) => i);
                }

                var data = context.IndustryDataByZips
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Join(zips, i => i.ZipCodeId, i => i.Id, (i, o) => i);
                   

             

                /*

                var data = context.BestPlacesToAdvertises.Where(i => i.Year == maxes.Year && i.Quarter == maxes.Quarter && i.IndustryId == industryId)
                    .Select(i => new Advertising()
                    {
                        ZipCodeId = i.ZipCode.Id,
                        Name = i.ZipCode.Name,
                        AverageRevenue = (long)i.AverageRevenue,
                        TotalRevenue = (long)i.TotalRevenue,
                        TotalEmployees = (long)i.totalEmployees,
                        RevenuePerCapita = (long)i.RevenuePerCapita,
                        BachelorsDegreeOrHigher = (int)i.BachelorsOrHigher,
                        HighSchoolOrHigher = (int)i.HighSchoolOrHigher,
                        MedianAge = (int)i.MedianAge,
                        HouseholdIncome = (long)i.MedianHouseholdIncome,
                        HouseholdExpendatures = (long)i.AverageHouseholdExpenditure,
                        WhiteCollarWorkers = (long)i.WhiteCollarWorkers
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

                */


                data = data.OrderByDescending(i=>i.TotalRevenue);


                //data = data.Join(zips, i => i.ZipCodeId, i => i.Id, (i, o) => i);


               // IQueryable<Advertising> rawData = data;
                /*if (distance != null)
                {
                    rawData = data.ToList().AsQueryable();
                }*/





                
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
