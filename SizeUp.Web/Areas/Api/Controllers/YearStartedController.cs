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
    public class YearStartedController : Controller
    {
        //
        // GET: /Api/YearStarted/

        public ActionResult YearStarted(long industryId, long cityId, long countyId)
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



                var nation = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var nMax = nation.Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenBy(i => i.Quarter);
                var nationData = nation.Where(i =>
                    i.Year == nMax.FirstOrDefault().Year && i.Quarter == nMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);


                var state = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.FirstOrDefault().State.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var sMax = state.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var stateData = state.Where(i =>
                    i.Year == sMax.FirstOrDefault().Year && i.Quarter == sMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var metro = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.FirstOrDefault().Metro.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var mMax = metro.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var metroData = metro.Where(i =>
                    i.Year == mMax.FirstOrDefault().Year && i.Quarter == mMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var county = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.CountyId == locations.FirstOrDefault().County.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var coMax = county.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var countyData = county.Where(i =>
                    i.Year == coMax.FirstOrDefault().Year && i.Quarter == coMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var city = context.YearStartedByCities
                    .Where(i => i.IndustryId == industryId && i.CityId == locations.FirstOrDefault().City.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var cMax = city.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var cityData = city.Where(i =>
                    i.Year == cMax.FirstOrDefault().Year && i.Quarter == cMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);


                var data = countyData.Select(i => new Models.Charts.LineChart()
                {
                    City = new Models.YearStarted.ChartItem()
                    {
                        Name = "City",
                        Values = cityData.GroupBy(g => g.Value).Select(g => new Models.YearStarted.ChartItem.ChartItemValue() { Key = g.Key, Value = g.Count() })
                    },
                    County = new Models.YearStarted.ChartItem()
                    {
                        Name = "County",
                        Values = countyData.GroupBy(g => g.Value).Select(g => new Models.YearStarted.ChartItem.ChartItemValue() { Key = g.Key, Value = g.Count() })
                    },
                    Metro = new Models.YearStarted.ChartItem()
                    {
                        Name = "Metro",
                        Values = metroData.GroupBy(g => g.Value).Select(g => new Models.YearStarted.ChartItem.ChartItemValue() { Key = g.Key, Value = g.Count() })
                    },
                    State = new Models.YearStarted.ChartItem()
                    {
                        Name = "State",
                        Values = stateData.GroupBy(g => g.Value).Select(g => new Models.YearStarted.ChartItem.ChartItemValue() { Key = g.Key, Value = g.Count() })
                    },
                    Nation = new Models.YearStarted.ChartItem()
                    {
                        Name = "Nation",
                        Values = nationData.GroupBy(g => g.Value).Select(g => new Models.YearStarted.ChartItem.ChartItemValue() { Key = g.Key, Value = g.Count() })
                    }
                }).FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, long cityId, long countyId, int value)
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



                var nation = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var nMax = nation.Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenBy(i => i.Quarter);
                var nationData = nation.Where(i =>
                    i.Year == nMax.FirstOrDefault().Year && i.Quarter == nMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);


                var state = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.FirstOrDefault().State.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var sMax = state.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var stateData = state.Where(i =>
                    i.Year == sMax.FirstOrDefault().Year && i.Quarter == sMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var metro = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.FirstOrDefault().Metro.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var mMax = metro.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var metroData = metro.Where(i =>
                    i.Year == mMax.FirstOrDefault().Year && i.Quarter == mMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var county = context.YearStartedByZips
                    .Where(i => i.IndustryId == industryId && i.CountyId == locations.FirstOrDefault().County.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var coMax = county.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var countyData = county.Where(i =>
                    i.Year == coMax.FirstOrDefault().Year && i.Quarter == coMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);

                var city = context.YearStartedByCities
                    .Where(i => i.IndustryId == industryId && i.CityId == locations.FirstOrDefault().City.Id)
                    .Select(i => new { i.Year, i.Quarter, i.YearStarted, i.YearEstablished });
                var cMax = city.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenBy(i => i.Quarter);
                var cityData = city.Where(i =>
                    i.Year == cMax.FirstOrDefault().Year && i.Quarter == cMax.FirstOrDefault().Quarter)
                    .Select(i => i.YearEstablished ?? i.YearStarted);



                var data = countyData.Select(i => new
                {
                    City = new
                    {
                        Total = cityData.Count(),
                        Less = cityData.Where(g=>g.Value < value).Count()
                    },
                    County = new
                    {
                        Total = countyData.Count(),
                        Less = countyData.Where(g => g.Value < value).Count()
                    },
                    Metro = new
                    {
                        Total = metroData.Count(),
                        Less = metroData.Where(g => g.Value < value).Count()
                    },
                    State = new
                    {
                        Total = stateData.Count(),
                        Less = stateData.Where(g => g.Value < value).Count()
                    },
                    Nation = new
                    {
                        Total = nationData.Count(),
                        Less = nationData.Where(g => g.Value < value).Count()
                    },
                }).FirstOrDefault();


                object obj = new 
                {
                    City = data.City.Total > 0 ? (int?)(((decimal)data.City.Less / (decimal)data.City.Total) * 100) : null,
                    County = data.County.Total > 0 ? (int?)(((decimal)data.County.Less / (decimal)data.County.Total) * 100) : null,
                    Metro = data.Metro.Total > 0 ? (int?)(((decimal)data.Metro.Less / (decimal)data.Metro.Total) * 100) : null,
                    State = data.State.Total > 0 ? (int?)(((decimal)data.State.Less / (decimal)data.State.Total) * 100) : null,
                    Nation = data.Nation.Total > 0 ? (int?)(((decimal)data.Nation.Less / (decimal)data.Nation.Total) * 100) : null
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
