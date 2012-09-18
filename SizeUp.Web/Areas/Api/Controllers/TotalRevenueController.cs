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
        public ActionResult TotalRevenue(long industryId, long placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();
                IQueryable<Models.TotalRevenue.ChartItem> m = null;
                var n = IndustryData.GetNational(context, industryId)
                                        .Where(i => i.TotalRevenue != null && i.TotalRevenue > 0)
                    .Select(i => new Models.TotalRevenue.ChartItem()
                    {
                        Value = (long)i.TotalRevenue,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Where(i => i.TotalRevenue != null && i.TotalRevenue > 0)
                    .Select(i => new Models.TotalRevenue.ChartItem()
                    {
                        Value = (long)i.TotalRevenue,
                        Name = locations.State.Name
                    });

                if (locations.Metro != null)
                {
                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                        .Where(i => i.TotalRevenue != null && i.TotalRevenue > 0)
                        .Select(i => new Models.TotalRevenue.ChartItem()
                        {
                            Value = (long)i.TotalRevenue,
                            Name = locations.Metro.Name
                        });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                    .Where(i => i.TotalRevenue != null && i.TotalRevenue > 0)
                   .Select(i => new Models.TotalRevenue.ChartItem()
                   {
                       Value = (long)i.TotalRevenue,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });

                var c = IndustryData.GetCity(context, industryId, locations.City.Id)
                    .Where(i => i.TotalRevenue != null && i.TotalRevenue > 0)
                   .Select(i => new Models.TotalRevenue.ChartItem()
                   {
                       Value = (long)i.TotalRevenue,
                       Name = locations.City.Name + ", " + locations.State.Abbreviation
                   });


                var data = new Models.Charts.BarChart()
                {
                    City = c.FirstOrDefault(),
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m == null ? null : m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };


                return Json(data, JsonRequestBehavior.AllowGet);
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
                        old.Max = band.Min;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
