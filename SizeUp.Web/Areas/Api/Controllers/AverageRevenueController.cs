using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AverageRevenueController : Controller
    {
        //
        // GET: /Api/AverageRevenue/

        public ActionResult AverageRevenue(long industryId, long cityId, long countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var locations = context.CityCountyMappings
                    .Select(i => new
                    {
                        City = i.City,
                        County = i.County,
                        Metro = i.County.Metro,
                        State = i.County.State
                    })
                    .Where(i => i.County.Id == countyId && i.City.Id == cityId);



                var nation = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId && i.Revenue > 0)
                    .GroupBy(i => new { i.Year, i.Quarter })
                    .Select(i => new { i.Key.Year, i.Key.Quarter, Revenue = i.Average(g => g.Revenue * 1000) });
                var nMax = nation.Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenBy(i =>i.Quarter);
                var nationData = nation.Where(i =>
                    i.Year == nMax.FirstOrDefault().Year && i.Quarter == nMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.Revenue,
                        Name = "USA"
                    });

               
                var state = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.FirstOrDefault().State.Id && i.Revenue > 0)
                    .GroupBy(i => new { i.Year, i.Quarter, i.StateId })
                    .Select(i => new { i.Key.Year, i.Key.Quarter, i.Key.StateId,  Revenue = i.Average(g => g.Revenue * 1000) });
                var sMax = state.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var stateData = state.Where(i =>
                    i.Year == sMax.FirstOrDefault().Year && i.Quarter == sMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.Revenue,
                        Name = locations.FirstOrDefault().State.Name
                    });

                var metro = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.FirstOrDefault().Metro.Id && i.Revenue > 0)
                    .GroupBy(i => new { i.Year, i.Quarter, i.MetroId })
                    .Select(i => new { i.Key.Year, i.Key.Quarter, i.Key.MetroId,  Revenue = i.Average(g => g.Revenue * 1000) });
                var mMax = metro.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var metroData = metro.Where(i =>
                    i.Year == mMax.FirstOrDefault().Year && i.Quarter == mMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.Revenue,
                        Name = locations.FirstOrDefault().Metro.Name
                    });

                var county = context.RevenueByZips
                    .Where(i => i.IndustryId == industryId && i.CountyId == locations.FirstOrDefault().County.Id && i.Revenue > 0)
                    .GroupBy(i => new { i.Year, i.Quarter, i.CountyId })
                    .Select(i => new { i.Key.Year, i.Key.Quarter, i.Key.CountyId, Revenue = i.Average(g => g.Revenue * 1000) });
                var coMax = county.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var countyData = county.Where(i =>
                    i.Year == coMax.FirstOrDefault().Year && i.Quarter == coMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.Revenue,
                        Name = locations.FirstOrDefault().County.Name + ", " + locations.FirstOrDefault().State.Abbreviation
                    });

                var city = context.RevenueByCities
                  .Where(i => i.IndustryId == industryId && i.CityId == locations.FirstOrDefault().City.Id && i.Revenue > 0)
                  .GroupBy(i => new { i.Year, i.Quarter, i.CityId })
                  .Select(i => new { i.Key.Year, i.Key.Quarter, i.Key.CityId, Revenue = i.Average(g => g.Revenue * 1000) });
                var cMax = city.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var cityData = city.Where(i =>
                    i.Year == cMax.FirstOrDefault().Year && i.Quarter == cMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.Revenue,
                        Name = locations.FirstOrDefault().City.Name + ", " + locations.FirstOrDefault().State.Abbreviation
                    });


                var data = countyData.Select(i => new Models.Charts.BarChart()
                {
                    City = cityData.FirstOrDefault(),
                    Nation = nationData.FirstOrDefault(),
                    State = stateData.FirstOrDefault(),
                    Metro = metroData.FirstOrDefault(),
                    County = i
                }).FirstOrDefault();
             

            

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, decimal value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
               

                var revenues = context.RevenueByZips
                    .Where(i =>i.IndustryId == industryId)
                    .Select(i => new { Revenue = i.Revenue*1000, i.Year, i.Quarter });

                var maxes = revenues
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);

                revenues = revenues.Where(i =>
                    i.Year == maxes.FirstOrDefault().Year &&
                        i.Quarter == maxes.FirstOrDefault().Quarter
                        );

                var data = new
                {
                    Total = revenues.Count(),
                    Less = revenues.Where(i => i.Revenue < value).Count()
                };

                object obj = null;
                if (data.Total > 0)
                {
                    obj = new
                    {
                        Percentile = (int)(((decimal)data.Less / (decimal)data.Total) * 100)
                    };
                }

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BandsByZip(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var filters = context.RevenueByZips
                  .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter)
                   .FirstOrDefault();

                filters = filters.Where(i => i.Year == maxes.Year && i.Quarter == maxes.Quarter);


                if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
                {
                    filters = filters.Where(i => i.County.Id == boundingEntity.EntityId);
                }

                var data = filters
                    .GroupBy(i => new { i.Year, i.Quarter, i.ZipCodeId })
                    .Select(i => new { i.Key.ZipCodeId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                    .ToList();

                var bandData = data.NTile(i => i.Revenue, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i.Revenue), Max = b.Max(i => i.Revenue) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in bandData)
                {
                    if (old != null)
                    {
                        old.Max = band.Min - 1;
                    }
                    old = band;
                }
                return Json(bandData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByCounty(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var filters = context.RevenueByZips
                  .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter)
                   .FirstOrDefault();

                filters = filters.Where(i => i.Year == maxes.Year && i.Quarter == maxes.Quarter);


                if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
                }
                else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
                }
                var data = filters
                    .GroupBy(i => new { i.Year, i.Quarter, i.CountyId })
                    .Select(i => new { i.Key.CountyId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                    .ToList();

                var bandData = data.NTile(i => i.Revenue, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i.Revenue), Max = b.Max(i => i.Revenue) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in bandData)
                {
                    if (old != null)
                    {
                        old.Max = band.Min - 1;
                    }
                    old = band;
                }
                return Json(bandData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByState(long industryId, int bands)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var filters = context.RevenueByZips
                  .Where(i => i.IndustryId == industryId && i.Revenue > 0);

                var maxes = filters
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter)
                   .FirstOrDefault();

                filters = filters.Where(i => i.Year == maxes.Year && i.Quarter == maxes.Quarter);

                var data = filters
                    .GroupBy(i => new { i.Year, i.Quarter, i.StateId })
                    .Select(i => new { i.Key.Year, i.Key.Quarter, i.Key.StateId, Revenue = (long)i.Average(g => g.Revenue * 1000) })
                    .ToList();

                var bandData = data.NTile(i => i.Revenue, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i.Revenue), Max = b.Max(i => i.Revenue) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
                foreach (var band in bandData)
                {
                    if (old != null)
                    {
                        old.Max = band.Min - 1;
                    }
                    old = band;
                }
                return Json(bandData, JsonRequestBehavior.AllowGet);
            }
        }
     
    }
}
