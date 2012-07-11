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
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Api/AverageSalary/

        public ActionResult AverageSalary(int industryId, int countyId)
        {

            using (var context = ContextFactory.SizeUpContext)
            {

                var locations = context.CityCountyMappings
                    .Select(i => new
                    {
                        County = i.County,
                        Metro = i.County.Metro,
                        State = i.County.State
                    })
                    .Where(i => i.County.Id == countyId).FirstOrDefault();




                var n = context.IndustryDataByNations
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.AverageSalary.ChartItem()
                    {
                        Value = (long)i.AverageAnnualSalary,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
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

                var geos = context.CountyGeographies
                    .Where(i => i.GeographyClass.Name == "Calculation" && i.Geography.GeographyPolygon.Intersects(boundingEntity.DbGeography.Buffer(-1)))
                    .Select(i => i.CountyId);

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageAnnualSalary > 0)
                    .Join(geos, i => i.CountyId, i => i, (i, o) => i)
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

                var data = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter && i.AverageAnnualSalary > 0)
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
