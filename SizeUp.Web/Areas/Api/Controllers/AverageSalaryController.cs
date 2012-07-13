using System;
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
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Api/AverageSalary/

        public ActionResult AverageSalary(int industryId, int countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var locations = Data.Views.Locations.Get(context, countyId).FirstOrDefault();

                var n = IndustryData.GetNational(context, industryId)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = "USA"
                    });

                var s = IndustryData.GetState(context, industryId, locations.State.Id)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = locations.State.Name
                    });

                var m = IndustryData.GetMetro(context, industryId, locations.Metro.Id)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = locations.Metro.Name
                    });

                var co = IndustryData.GetCounty(context, industryId, locations.County.Id)
                   .Select(i => new Models.AverageSalary.ChartItem()
                   {
                       Value = (long)i.AverageAnnualSalary,
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

        public ActionResult Percentage(int industryId, int countyId, decimal value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var salary = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.CountyId == countyId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.AverageAnnualSalary)
                    .FirstOrDefault();

                object obj = null;
                if (salary!= null && salary != 0)
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
                        old.Max = band.Min - 1;
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
                        old.Max = band.Min - 1;
                    }
                    old = band;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
