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
using SizeUp.Core;



namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TotalRevenueController : Controller
    {
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
                    .Select(i => i.TotalRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.TotalRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.TotalRevenue.Band old = null;
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
                    .Select(i => i.TotalRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.TotalRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.TotalRevenue.Band old = null;
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
                    .Select(i => i.TotalRevenue)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.TotalRevenue.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.TotalRevenue.Band old = null;
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
