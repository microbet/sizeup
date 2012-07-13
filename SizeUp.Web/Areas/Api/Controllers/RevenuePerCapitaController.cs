using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Data.Views;
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

        public ActionResult RevenuePerCapita(int industryId, int countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, countyId).FirstOrDefault();




                var n = context.IndustryDataByNations
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.RevenuePerCapita,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.RevenuePerCapita,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.RevenuePerCapita,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.RevenuePerCapita.ChartItem()
                   {
                       Value = i.RevenuePerCapita,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var data = new Models.Charts.BarChart()
                {
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Percentile(long industryId, int countyId, int cityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var value = context.IndustryDataByCities
                   .Where(i => i.IndustryId == industryId && i.CityId == cityId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => i.RevenuePerCapita)
                   .FirstOrDefault();

                var cities = context.IndustryDataByCities
                   .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => i.RevenuePerCapita);

                var data = new
                {
                    Total = cities.Count(),
                    Less = cities.Where(i => i.Value < value).Count()
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


        public ActionResult Percentage(int industryId, int countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var cities = context.IndustryDataByCities
                   .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => i.RevenuePerCapita);

                var counties = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.RevenuePerCapita);

                var metros = context.IndustryDataByMetroes
                   .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => i.RevenuePerCapita);

                var states = context.IndustryDataByStates
                   .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => i.RevenuePerCapita);

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.CountyId == countyId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.RevenuePerCapita)
                    .FirstOrDefault();

                object obj = null;
               /* if (data != null && value != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - data) / data) * 100)
                    };
                }*/
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult BandsByZip(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
                IQueryable<long> zips = context.ZipCodes.Select(i => i.Id);

                if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.City)
                {
                    zips = context.ZipCodeCityMappings
                        .Where(i => i.CityId == boundingEntity.EntityId)
                        .Select(i => i.ZipCodeId);
                }
                else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
                {
                    zips = context.ZipCodeCountyMappings
                       .Where(i => i.CountyId == boundingEntity.EntityId)
                       .Select(i => i.ZipCodeId);
                }
                else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    zips = context.ZipCodeCountyMappings
                       .Where(i => i.County.MetroId == boundingEntity.EntityId)
                       .Select(i => i.ZipCodeId);
                }
                else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    zips = context.ZipCodeCountyMappings
                       .Where(i => i.County.StateId == boundingEntity.EntityId)
                       .Select(i => i.ZipCodeId);
                }


                var data = context.IndustryDataByZips
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
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
                        old.Max = band.Min - 1;
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
                IQueryable<long> ids = context.Counties.Select(i => i.Id);

                if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
                {
                    ids = context.Counties
                       .Where(i => i.MetroId == boundingEntity.EntityId)
                       .Select(i => i.Id);
                }
                else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
                {
                    ids = context.Counties
                       .Where(i => i.StateId == boundingEntity.EntityId)
                       .Select(i => i.Id);
                }


                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
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
                        old.Max = band.Min - 1;
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

                var data = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
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
                        old.Max = band.Min - 1;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
