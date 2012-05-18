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
 

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AverageSalaryController : Controller
    {
        //
        // GET: /Api/AverageSalary/

        public ActionResult Salary(int industryId, int countyId)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();

            var locations = DataContexts.SizeUpContext.Counties.Where(i => i.Id == countyId)
                .Select(i => new
                {
                    County = i,
                    Metro = i.Metro,
                    State = i.State
                })
                .FirstOrDefault();

            long? national = null;
            long? state = null;
            long? metro = null;
            long? county = null;

            national = DataContexts.SizeUpContext.AverageSalaryNationals.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryNationals.Max(m => m.Year) && i.NAICSId == naics.Id && i.AverageSalary > 0).Select(i => i.AverageSalary).FirstOrDefault();
            state = DataContexts.SizeUpContext.AverageSalaryByStates.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByStates.Max(m => m.Year) && i.NAICSId == naics.Id && i.StateId == locations.State.Id && i.AverageSalary > 0).Select(i => i.AverageSalary).FirstOrDefault();
            if (locations.Metro != null)
            {
                metro = DataContexts.SizeUpContext.AverageSalaryByMetroes.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByMetroes.Max(m => m.Year) && i.NAICSId == naics.Id && i.MetroId == locations.Metro.Id && i.AverageSalary > 0).Select(i => i.AverageSalary).FirstOrDefault();
            }
            county = DataContexts.SizeUpContext.AverageSalaryByCounties.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id && i.CountyId == locations.County.Id && i.AverageSalary > 0).Select(i => i.AverageSalary).FirstOrDefault();

            var obj = new Models.Charts.BarChart();
            obj.Nation = new Models.Charts.ChartItem()
            {
                Value = national.HasValue ? national.Value.ToString() : null,
                Name = "USA"
            };
            obj.State = new Models.Charts.ChartItem()
            {
                Value = state.HasValue ? state.Value.ToString() : null,
                Name = locations.State.Name
            };
            if (locations.Metro != null && metro!=null)
            {
                obj.Metro = new Models.Charts.ChartItem()
                {
                    Value = metro.HasValue ? metro.Value.ToString() : null,
                    Name = locations.Metro.Name
                };
            }
            obj.County = new Models.Charts.ChartItem()
            {
                Value = county.HasValue ? county.Value.ToString() : null,
                Name = string.Format("{0}, {1}", locations.County.Name, locations.State.Abbreviation)
            };


            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Percentile(int industryId, int countyId, decimal value)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            long? county = DataContexts.SizeUpContext.AverageSalaryByCounties.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id && i.CountyId == countyId).Select(i => i.AverageSalary).FirstOrDefault();
            object obj = null;
            if (county.HasValue && county != 0)
            {
                obj = new
                {
                    Percentile = (int)(((value - county) / county) * 100)
                };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BandsByCounty(int industryId, int bands, string boundingEntityId)
        {
            BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            var filters = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id && i.AverageSalary > 0);

            if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                filters = filters.Where(i => i.County.StateId == boundingEntity.EntityId);
            }
            else if (boundingEntity.EntityType != null && boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                filters = filters.Where(i => i.County.MetroId == boundingEntity.EntityId);
            }
            var data = filters.Select(i => new { i.AverageSalary, i.CountyId }).ToList();
            var bandData = data.NTile(i => i.AverageSalary, bands)
                .Select(b => new Models.Salary.Band(){ Min = b.Min(i => i.AverageSalary), Max = b.Max(i => i.AverageSalary) })
                .ToList();

            Models.Salary.Band old = null;
            foreach(var band in bandData)
            {
                if (old != null)
                {
                    old.Max = band.Min - 1;
                }
                old = band;
            }
            return Json(bandData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BandsByState(int industryId, int bands)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            var filters = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByStates.Max(m => m.Year) && i.NAICSId == naics.Id && i.AverageSalary > 0);

            var data = filters.Select(i => new { i.AverageSalary, i.CountyId }).ToList();
            var bandData = data.NTile(i => i.AverageSalary, bands)
                .Select(b => new Models.Salary.Band() { Min = b.Min(i => i.AverageSalary), Max = b.Max(i => i.AverageSalary) })
                .ToList();

            Models.Salary.Band old = null;
            foreach (var band in bandData)
            {
                if (old != null)
                {
                    old.Max = band.Min - 1;
                }
                old = band;
            }
            return Json(bandData, JsonRequestBehavior.AllowGet);
        }

    }
}
