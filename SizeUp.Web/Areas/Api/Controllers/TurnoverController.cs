using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;



namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TurnoverController : Controller
    {
        //
        // GET: /Api/Turnover/

        public ActionResult Turnover(int industryId, int countyId)
        {
            var naics4 = DataContexts.SizeUpContext.Industries.Where(i => i.Id == industryId)
               .Select(i => DataContexts.SizeUpContext.NAICS.Where(n => n.NAICSCode == DataContexts.SizeUpContext.SicToNAICSMappings
                       .Where(m => m.IndustryId == i.Id)
                       .Select(m => m.NAICS)
                       .FirstOrDefault()
                       .NAICSCode.Substring(0, 4)
                  ).FirstOrDefault()
               ).FirstOrDefault();

            var locations = DataContexts.SizeUpContext.Counties.Where(i => i.Id == countyId)
                .Select(i => new
                {
                    County = i,
                    Metro = i.Metro,
                    State = i.State
                })
                .FirstOrDefault();




            Api.Models.Turnover.TurnoverDynamics nation = null;
            Api.Models.Turnover.TurnoverDynamics state = null;
            Api.Models.Turnover.TurnoverDynamics metro = null;
            Api.Models.Turnover.TurnoverDynamics county = null;



            nation = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByStates.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByStates.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id)
                .GroupBy(g => new { g.Year, g.Quarter })
                .Select(i => new Api.Models.Turnover.TurnoverDynamics() { Turnover = (0.5d * (i.Sum(g=>g.Hires) + i.Sum(g=>g.Separations)) / i.Sum(g=>g.Employment)) *100, Hires = i.Sum(g=>g.Hires), Separations = i.Sum(g=>g.Separations) })
                .FirstOrDefault();

            state = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByStates.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByStates.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id && i.StateId == locations.State.Id)
                .Select(i => new Api.Models.Turnover.TurnoverDynamics() { Turnover = i.Turnover*100, Hires = i.Hires, Separations = i.Separations })
                .FirstOrDefault();

            if (locations.Metro != null)
            {
                metro = DataContexts.SizeUpContext.LaborDynamicsByMetroes.Where(i =>
                    i.Year == DataContexts.SizeUpContext.LaborDynamicsByMetroes.Max(m => m.Year) &&
                    i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByMetroes.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                    i.NAICSId == naics4.Id && i.MetroId == locations.Metro.Id)
                    .Select(i => new Api.Models.Turnover.TurnoverDynamics() { Turnover = i.Turnover*100, Hires = i.Hires, Separations = i.Separations })
                    .FirstOrDefault();
            }

            county = DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByCounties.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id && i.CountyId == locations.County.Id)
                .Select(i => new Api.Models.Turnover.TurnoverDynamics() { Turnover = i.Turnover*100, Hires = i.Hires, Separations = i.Separations })
                .FirstOrDefault();


            var obj = new Models.Charts.BarChart();
            if (nation != null)
            {
                obj.Nation = new Models.Turnover.ChartItem()
                {
                    Hires = nation.Hires,
                    Separations = nation.Separations,
                    Turnover = nation.Turnover,
                    Name = "USA"
                };
            }
            if (state != null)
            {
                obj.State = new Models.Turnover.ChartItem()
                {
                    Hires = state.Hires,
                    Separations = state.Separations,
                    Turnover = state.Turnover,
                    Name = locations.State.Name
                };
            }
            if (locations.Metro != null && metro != null)
            {
                obj.Metro = new Models.Turnover.ChartItem()
                {
                    Hires = metro.Hires,
                    Separations = metro.Separations,
                    Turnover = metro.Turnover,
                    Name = locations.Metro.Name
                };
            }
            if (county != null)
            {
                obj.County = new Models.Turnover.ChartItem()
                {
                    Hires = county.Hires,
                    Separations = county.Separations,
                    Turnover = county.Turnover,
                    Name = string.Format("{0}, {1}", locations.County.Name, locations.State.Abbreviation)
                };
            }




            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Percentile(int industryId, int countyId)
        {
            var naics4 = DataContexts.SizeUpContext.Industries.Where(i => i.Id == industryId)
               .Select(i => DataContexts.SizeUpContext.NAICS.Where(n => n.NAICSCode == DataContexts.SizeUpContext.SicToNAICSMappings
                       .Where(m => m.IndustryId == i.Id)
                       .Select(m => m.NAICS)
                       .FirstOrDefault()
                       .NAICSCode.Substring(0, 4)
                  ).FirstOrDefault()
               ).FirstOrDefault();

            var turnovers = DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByCounties.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id)
                .Select(i => new { i.Turnover, i.CountyId });


            var currentTurnover = turnovers.Where(i => i.CountyId == countyId).Select(i => i.Turnover).FirstOrDefault();
            var total = turnovers.Count();
            var less = turnovers.Where(i => i.Turnover < currentTurnover).Count();

            object obj = null;
            if (total > 0)
            {
                obj = new
                {
                    Percentile = (int)((less / total) * 100)
                };
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
