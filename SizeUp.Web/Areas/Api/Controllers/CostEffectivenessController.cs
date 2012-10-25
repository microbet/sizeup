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
    public class CostEffectivenessController : BaseController
    {
        //
        // GET: /Api/AverageRevenue/

        public ActionResult CostEffectiveness(long industryId, long placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();
                IQueryable<Models.CostEffectiveness.ChartItem> m = null;
                var n = IndustryData.GetNational(context,industryId)
                    .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                    .Select(i => new Models.CostEffectiveness.ChartItem()
                    {
                        Value = (double)i.CostEffectiveness,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                    .Select(i => new Models.CostEffectiveness.ChartItem()
                    {
                        Value = (double)i.CostEffectiveness,
                        Name = locations.State.Name
                    });

                if (locations.Metro != null)
                {
                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                        .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                        .Select(i => new Models.CostEffectiveness.ChartItem()
                        {
                            Value = (double)i.CostEffectiveness,
                            Name = locations.Metro.Name
                        });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                   .Select(i => new Models.CostEffectiveness.ChartItem()
                   {
                       Value = (double)i.CostEffectiveness,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });


                var data = new Models.Charts.BarChart()
                {
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m == null ? null : m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };
            

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, int placeId, double revenue, int employees, double salary)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var ce = revenue / (double)(employees * salary);


                var county = IndustryData.GetCounty(context, industryId, locations.County.Id)
                    .Select(i => i.CostEffectiveness)
                    .FirstOrDefault();

                object obj = null;
                if (county != null && county != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((ce - county) / county) * 100)
                    };
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
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
                    .Where(i => i.CostEffectiveness > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.CostEffectiveness)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.CostEffectiveness.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.CostEffectiveness.Band old = null;
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
                    .Where(i => i.CostEffectiveness > 0)
                    .Select(i => i.CostEffectiveness)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.CostEffectiveness.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.CostEffectiveness.Band old = null;
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
