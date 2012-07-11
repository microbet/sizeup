using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
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
                    .Where(i => i.County.Id == countyId && i.City.Id == cityId).FirstOrDefault();




                var n = context.IndustryDataByNations
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.AverageRevenue.ChartItem()
                   {
                       Value = (long)i.AverageRevenue,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var c = context.IndustryDataByCities
                   .Where(i => i.IndustryId == industryId && i.CityId == locations.City.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.AverageRevenue.ChartItem()
                   {
                       Value = (long)i.AverageRevenue,
                       Name = locations.City.Name + ", " + locations.State.Abbreviation
                   });


                var data = new Models.Charts.BarChart()
                {
                    City = c.FirstOrDefault(),
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };
            

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, decimal value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var revenues = context.IndustryDataByCities
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.AverageRevenue);

                var data = new
                {
                    Total = revenues.Count(),
                    Less = revenues.Where(i => i.Value < value).Count()
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

                var geos = context.ZipCodeGeographies
                    .Where(i => i.GeographyClass.Name == "Calculation" && i.Geography.GeographyPolygon.Intersects(boundingEntity.DbGeography.Buffer(-1)))
                    .Select(i => i.ZipCodeId);
                   
                var data = context.IndustryDataByZips
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
                    .Join(geos, i => i.ZipCodeId, i => i, (i, o) => i)
                    .Select(i=>i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();
           
                Models.AverageRevenue.Band old = null;
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

                var geos = context.CountyGeographies
                    .Where(i => i.GeographyClass.Name == "Calculation" && i.Geography.GeographyPolygon.Intersects(boundingEntity.DbGeography.Buffer(-1)))
                    .Select(i => i.CountyId);

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageRevenue > 0)
                    .Join(geos, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
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
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageRevenue.Band old = null;
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
