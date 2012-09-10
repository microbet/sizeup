using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;



namespace SizeUp.Web.Areas.Api.Controllers
{
    public class RevenuePerCapitaController : Controller
    {
        //
        // GET: /Api/RevenuePerCapita/

        public ActionResult RevenuePerCapita(int industryId, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                IQueryable<Models.RevenuePerCapita.ChartItem> m = null;
                var n = IndustryData.GetNational(context, industryId)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.RevenuePerCapita,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                     .Select(i => new Models.RevenuePerCapita.ChartItem()
                     {
                         Value = i.RevenuePerCapita,
                         Name = locations.State.Name
                     });
                if (locations.Metro != null)
                {
                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                         .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                        .Select(i => new Models.RevenuePerCapita.ChartItem()
                        {
                            Value = i.RevenuePerCapita,
                            Name = locations.Metro.Name
                        });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => new Models.RevenuePerCapita.ChartItem()
                   {
                       Value = i.RevenuePerCapita,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var c = IndustryData.GetCity(context, industryId, locations.City.Id)
                  .Select(i => new Models.RevenuePerCapita.ChartItem()
                  {
                      Value = (long)i.RevenuePerCapita,
                      Name = locations.City.Name + ", " + locations.State.Abbreviation
                  });


                var data = new Models.Charts.BarChart()
                {
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m == null ? null : m.FirstOrDefault(),
                    County = co.FirstOrDefault(),
                    City = c.FirstOrDefault()
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Percentile(long industryId, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                IQueryable<long?> metro = null;
                var raw = IndustryData.GetCities(context, industryId);

                var coIds = Cities.GetBounded(context, locations.CountyBoundingEntity)
                    .Select(i=>i.Id);
                var sIds = Cities.GetBounded(context, locations.StateBoundingEntity)
                    .Select(i=>i.Id);

                if (locations.Metro != null)
                {

                    var mIds = Cities.GetBounded(context, locations.MetroBoundingEntity)
                      .Select(i => i.Id);
                    metro = raw.Join(mIds, i => i.CityId, i => i, (i, o) => i.RevenuePerCapita);
                }

                var county = raw.Join(coIds, i => i.CityId, i => i, (i, o) => i.RevenuePerCapita);    
                var state = raw.Join(sIds, i => i.CityId, i => i, (i, o) => i.RevenuePerCapita);
                var nation = raw.Select(i=>i.RevenuePerCapita);

                var value = IndustryData.GetCity(context, industryId, locations.City.Id)
                   .Select(i => i.RevenuePerCapita)
                   .FirstOrDefault();

                object obj = null;
                if (value != null)
                {
                    obj = new
                    {
                        County = Core.DataAccess.Math.Percentile(county, (long)value),
                        Metro = metro == null ? null : Core.DataAccess.Math.Percentile(metro, (long)value),
                        State = Core.DataAccess.Math.Percentile(state, (long)value),
                        Nation = Core.DataAccess.Math.Percentile(nation, (long)value)
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

                var zips = ZipCodes.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetZipCodes(context, industryId)
                    .Where(i => i.RevenuePerCapita > 0)
                    .Join(zips, i => i.ZipCodeId, i => i, (i, o) => i)
                    .Select(i => i.RevenuePerCapita)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.RevenuePerCapita.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.RevenuePerCapita.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByCounty(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.RevenuePerCapita > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.RevenuePerCapita)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.RevenuePerCapita.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.RevenuePerCapita.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BandsByState(long industryId, int bands)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var data = IndustryData.GetStates(context, industryId)
                    .Where(i => i.RevenuePerCapita > 0)
                    .Select(i => i.RevenuePerCapita)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.RevenuePerCapita.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.RevenuePerCapita.Band old = null;
                foreach (var band in data)
                {
                    if (old != null)
                    {
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
     
    }
}
