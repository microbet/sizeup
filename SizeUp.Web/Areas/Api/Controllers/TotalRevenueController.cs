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
using SizeUp.Core.DataAccess;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TotalRevenueController : Controller
    {

        public ActionResult BandsByZip(long industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var zips = ZipCodes.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetZipCodes(context, industryId)
                    .Where(i => i.TotalRevenue > 0)
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

                var ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.TotalRevenue > 0)
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

                var data = IndustryData.GetStates(context, industryId)
                    .Where(i => i.TotalRevenue > 0)
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
