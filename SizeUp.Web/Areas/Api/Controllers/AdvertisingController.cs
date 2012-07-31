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

            

            using (var context = ContextFactory.SizeUpContext)
            {
                var center = Locations.Get(context, placeId)
                    .Select(i=>i.City.CityGeographies.Where(g=>g.GeographyClass.Name == "Calculation").Select(g=>new { lat = g.Geography.CenterLat, lng = g.Geography.CenterLong}).FirstOrDefault())
                    .FirstOrDefault();

                var data = IndustryData.GetZipCodes(context, industryId);


            
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


                if (distance != null)
                {
                    var zips = ZipCodes.GetWithin(context, center.lat.Value, center.lng.Value, distance.Value);
                    data = data.Join(zips, i => i.ZipCodeId, i => i.Id, (i, o) => i);
                }




                data = data.OrderByDescending(i => i.TotalRevenue);



                var output = new
                {
                    Total = data.Count(),
                    Items = data
                        .Skip((page - 1) * itemCount)
                        .Take(itemCount)
                        .Select(i => new Advertising()
                        {
                            ZipCodeId = i.ZipCode.Id,
                            Name = i.ZipCode.Name,
                            AverageRevenue = (long)i.AverageRevenue,
                            TotalRevenue = (long)i.TotalRevenue,
                            TotalEmployees = (long)i.TotalEmployees,
                            RevenuePerCapita = (long)i.RevenuePerCapita,
                            BachelorsDegreeOrHigher = (int)i.BachelorsOrHigher,
                            HighSchoolOrHigher = (int)i.HighSchoolOrHigher,
                            MedianAge = (int)i.MedianAge,
                            HouseholdIncome = (long)i.MedianHouseholdIncome,
                            HouseholdExpendatures = (long)i.AverageHouseholdExpenditure,
                            WhiteCollarWorkers = (long)i.WhiteCollarWorkers
                        })
                        .ToList()
                };
                 
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
