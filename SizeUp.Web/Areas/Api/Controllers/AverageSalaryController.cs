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
    public class AverageSalaryController : BaseController
    {
        //
        // GET: /Api/AverageSalary/

        public ActionResult AverageSalary(int industryId, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var locations = Locations.Get(context, placeId).FirstOrDefault();
                IQueryable<Models.AverageSalary.ChartItem> m = null;
                var n = IndustryData.GetNational(context, industryId)
                    .Where(i => i.AverageAnnualSalary != null && i.AverageAnnualSalary > 0)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Where(i=>i.AverageAnnualSalary!=null && i.AverageAnnualSalary> 0)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = locations.State.Name
                    });

                if (locations.Metro != null)
                {
                    m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                        .Where(i => i.AverageAnnualSalary != null && i.AverageAnnualSalary > 0)
                        .Select(i => new Models.AverageSalary.ChartItem()
                        {
                            Value = (long)i.AverageAnnualSalary,
                            Name = locations.Metro.Name
                        });
                }

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Where(i => i.AverageAnnualSalary != null && i.AverageAnnualSalary > 0)
                   .Select(i => new Models.AverageSalary.ChartItem()
                   {
                       Value = (long)i.AverageAnnualSalary,
                       Name = locations.County.Name + ", " + locations.State.Abbreviation
                   });


                var data = new Models.Charts.BarChart()
                {
                    Nation = n.FirstOrDefault(),
                    State = s.FirstOrDefault(),
                    Metro = m == null ? null :  m.FirstOrDefault(),
                    County = co.FirstOrDefault()
                };


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, int placeId, decimal value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, placeId).FirstOrDefault();

                var salary = IndustryData.GetCounty(context, industryId, locations.County.Id)
                    .Where(i => i.AverageAnnualSalary != null && i.AverageAnnualSalary > 0)
                    .Select(i => i.AverageAnnualSalary)
                    .FirstOrDefault();

                object obj = null;
                if (salary != null && salary != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - salary) / salary) * 100)
                    };
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }


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

    }
}
