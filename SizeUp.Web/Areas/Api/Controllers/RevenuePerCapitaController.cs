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
    public class RevenuePerCapitaController : Controller
    {
        //
        // GET: /Api/RevenuePerCapita/

        public ActionResult RevenuePerCapita(int industryId, int countyId)
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
            obj.Nation = new Models.AverageSalary.ChartItem()
            {
                Value = national,
                Name = "USA"
            };
            obj.State = new Models.AverageSalary.ChartItem()
            {
                Value = state,
                Name = locations.State.Name
            };
            if (locations.Metro != null && metro != null)
            {
                obj.Metro = new Models.AverageSalary.ChartItem()
                {
                    Value = metro,
                    Name = locations.Metro.Name
                };
            }
            obj.County = new Models.AverageSalary.ChartItem()
            {
                Value = county,
                Name = string.Format("{0}, {1}", locations.County.Name, locations.State.Abbreviation)
            };


            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Percentage(int industryId, int countyId, decimal value)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            long? county = DataContexts.SizeUpContext.AverageSalaryByCounties.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id && i.CountyId == countyId).Select(i => i.AverageSalary).FirstOrDefault();
            object obj = null;
            if (county.HasValue && county != 0)
            {
                obj = new
                {
                    Percentage = (int)(((value - county) / county) * 100)
                };
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
