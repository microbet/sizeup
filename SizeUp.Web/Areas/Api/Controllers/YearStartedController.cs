using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
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

        public ActionResult YearStarted(long industryId, long placeId, int startYear, int endYear)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var years = Enumerable.Range(startYear, (endYear-startYear)+1).ToList();


                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var city = BusinessData.GetByCity(context, industryId, locations.City.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared)
                    .Where(i => i != null)
                    .Where(i => i >= startYear && i <= endYear);


                var county = BusinessData.GetByCounty(context, industryId, locations.County.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared)
                    .Where(i => i != null)
                    .Where(i => i >= startYear && i <= endYear);

                var metro = BusinessData.GetByMetro(context, industryId, locations.Metro.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared)
                    .Where(i => i != null)
                    .Where(i => i >= startYear && i <= endYear);

                var state = BusinessData.GetByState(context, industryId, locations.State.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared)
                    .Where(i => i != null)
                    .Where(i => i >= startYear && i <= endYear);

                var nation = BusinessData.GetByNation(context, industryId)
                    .Select(i => i.YearEstablished ?? i.YearAppeared)
                    .Where(i => i != null)
                    .Where(i => i >= startYear && i <= endYear);



                var obj = new Models.Charts.LineChart<int, int>()
                {
                    City = years.GroupJoin(city, o => o, i => i, (i, o) => new Models.Charts.LineChartItem<int, int>() { Key = i, Value = o.Count() }).ToList(),
                    County = years.GroupJoin(county, o => o, i => i, (i, o) => new Models.Charts.LineChartItem<int, int>() { Key = i, Value = o.Count() }).ToList(),
                    Metro = years.GroupJoin(metro, o => o, i => i, (i, o) => new Models.Charts.LineChartItem<int, int>() { Key = i, Value = o.Count() }).ToList(),
                    State = years.GroupJoin(state, o => o, i => i, (i, o) => new Models.Charts.LineChartItem<int, int>() { Key = i, Value = o.Count() }).ToList(),
                    Nation = years.GroupJoin(nation, o => o, i => i, (i, o) => new Models.Charts.LineChartItem<int, int>() { Key = i, Value = o.Count() }).ToList()
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, long placeId, int? value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();


                var city = BusinessData.GetByCity(context, industryId, locations.City.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared )
                    .Where(i => i != null);

                var county = BusinessData.GetByCounty(context, industryId, locations.County.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared )
                    .Where(i => i != null);

                var metro = BusinessData.GetByMetro(context, industryId, locations.Metro.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared )
                    .Where(i => i != null);

                var state = BusinessData.GetByState(context, industryId, locations.State.Id)
                    .Select(i => i.YearEstablished ?? i.YearAppeared )
                    .Where(i => i != null);

                var nation = BusinessData.GetByNation(context, industryId)
                    .Select(i => i.YearEstablished ?? i.YearAppeared )
                    .Where(i => i != null);



                var obj = new
                {
                    City = Core.DataAccess.Math.Percentile(city, value, Core.DataAccess.Math.Order.GreaterThan),
                    County = Core.DataAccess.Math.Percentile(county, value, Core.DataAccess.Math.Order.GreaterThan),
                    Metro = Core.DataAccess.Math.Percentile(metro, value, Core.DataAccess.Math.Order.GreaterThan),
                    State = Core.DataAccess.Math.Percentile(state, value, Core.DataAccess.Math.Order.GreaterThan),
                    Nation = Core.DataAccess.Math.Percentile(nation, value, Core.DataAccess.Math.Order.GreaterThan)
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }



    }
}
