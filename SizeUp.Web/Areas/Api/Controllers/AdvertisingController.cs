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



namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AdvertisingController : Controller
    {
        //
        // GET: /Api/Advertising/


        public ActionResult Advertising(int industryId, long cityId, int page = 1, int itemCount = 20)
        {
            if (!User.Identity.IsAuthenticated)
            {
                page = 1;
                itemCount = 3;
            }
            //pull out all parameters and arrays Request.QueryString.GetValues("pa")
            //build out a query that incorperates all the variables we would want to return and filter on
            //apply filters where nessesary

            using (var context = ContextFactory.SizeUpContext)
            {
                var maxes = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId)
                    .Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenBy(i => i.Quarter)
                    .FirstOrDefault();

                var classId = context.ZipCodeGeographies.Where(i => i.GeographyClass.Name == "Calculation").Select(i => i.ClassId).FirstOrDefault();

                var cityCenter = context.CityGeographies
                    .Where(i => i.CityId == cityId && i.ClassId == classId)
                    .Select(i => DbGeography.FromBinary(DbGeometry.FromBinary(i.Geography.GeographyPolygon.AsBinary()).Centroid.AsBinary(), 4326))
                    .FirstOrDefault();

                var zips = context.ZipCodeGeographies.Where(i => i.ClassId == classId);




                //if distance
                int radius = 20;
                var radiusMeters = radius * 1609.344;
                zips = zips.Where(i=> i.Geography.GeographyPolygon.Distance(cityCenter) < radiusMeters);


               //    .Select(i => i.ZipCodeId);







                //here is where we get our massive view on all parameters

                //or perhaps we build up independent queries and then do a join at the end
                var data = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId && zips.Contains(i.ZipCodeId) && i.Year == maxes.Year && i.Quarter == maxes.Quarter)
                    .GroupBy(i => new { i.ZipCodeId, i.ZipCode.Name })
                    .Select(i => new { i.Key.ZipCodeId, i.Key.Name, Revenue = (long)i.Sum(g => g.Revenue * 1000) })
                    .OrderByDescending(i => i.Revenue)
                    .ToList(); ///sad panda sql query optimizer sucks









                var output = new
                {
                    Total = data.Count(),
                    Items = data
                        .Skip((page - 1) * itemCount)
                        .Take(itemCount)
                        .Select(i => new
                        {
                            i.Revenue,
                            i.Name,
                            i.ZipCodeId
                        }).ToList()
                };


                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
