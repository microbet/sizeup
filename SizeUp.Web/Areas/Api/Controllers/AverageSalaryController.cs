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
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AverageSalaryController : BaseController
    {
        //
        // GET: /Api/AverageSalary/

        public ActionResult AverageSalary(int industryId, int placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageSalary.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, int placeId, int value, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.AverageSalary.Percentage(context, industryId, placeId, value, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bands(long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity = Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageSalary.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        /*
        public ActionResult BandsByCounty(int industryId, int bands, string boundingEntityId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);

                var ids = Counties.GetBounded(context, boundingEntity)
                    .Select(i => i.Id);

                var data = IndustryData.GetCounties(context, industryId)
                    .Where(i => i.AverageAnnualSalary > 0)
                    .Join(ids, i => i.CountyId, i => i, (i, o) => i)
                    .Select(i => i.AverageAnnualSalary)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageSalary.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageSalary.Band old = null;
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


        public ActionResult BandsByState(int industryId, int bands)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var data = IndustryData.GetStates(context, industryId)
                    .Where(i => i.AverageAnnualSalary > 0)
                    .Select(i => i.AverageAnnualSalary)
                    .ToList()
                    .NTile(i => i, bands)
                    .Select(b => new Models.AverageSalary.Band() { Min = b.Min(i => i), Max = b.Max(i => i) })
                    .ToList();

                Models.AverageSalary.Band old = null;
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
        */
    }
}
