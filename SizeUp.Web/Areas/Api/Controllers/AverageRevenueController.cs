﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Data.Views;
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

                var locations = Data.Views.Locations.Get(context, cityId, countyId).FirstOrDefault();

                var n = IndustryData.GetNational(context,industryId)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Median = i.MedianRevenue,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Name = locations.State.Name
                    });

                var m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                    .Select(i => new Models.AverageRevenue.ChartItem()
                    {
                        Value = (long)i.AverageRevenue,
                        Name = locations.Metro.Name
                    });

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => new Models.AverageRevenue.ChartItem()
                   {
                       Value = (long)i.AverageRevenue,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var c = IndustryData.GetCity(context, industryId, locations.City.Id)
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

                var revenues = IndustryData.GetCities(context, industryId)
                    .Select(i => i.AverageRevenue)
                    .Where(i => i != null);

                var percentile = revenues.Percentile(i => i.Value, value);

                var obj = new
                {
                    Percentile = percentile
                };
              
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BandsByZip(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
                IQueryable<long> zips = context.ZipCodes.Select(i=>i.Id);

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


                var data = IndustryData.GetZipCodes(context, industryId)
                    .Where(i => i.AverageRevenue > 0)
                    .Join(zips, i => i.ZipCodeId, i => i, (i, o) => i)
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


                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.AverageRevenue > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
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

                var data = IndustryData.GetStates(context, industryId)
                    .Where(i =>  i.AverageRevenue > 0)
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
