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
            using (var context = ContextFactory.SizeUpContext)
            {
                var naics4 =context.Industries.Where(i => i.Id == industryId)
                   .Select(i => context.NAICS.Where(n => n.NAICSCode == context.SicToNAICSMappings
                           .Where(m => m.IndustryId == i.Id)
                           .Select(m => m.NAICS)
                           .FirstOrDefault()
                           .NAICSCode.Substring(0, 4)
                      ).FirstOrDefault()
                   );

                var locations = context.Counties
                    .Select(i => new
                    {
                        County = i,
                        Metro = i.Metro,
                        State = i.State
                    })
                    .Where(i => i.County.Id == countyId);



                //nation
                var nation = context.LaborDynamicsByStates.Where(i => i.NAICSId == naics4.FirstOrDefault().Id);
                var nMax = nation.Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenByDescending(i => i.Quarter);
                var nationData = nation.Where(i =>
                    i.Year == nMax.FirstOrDefault().Year &&
                    i.Quarter == nMax.FirstOrDefault().Quarter)

                   .GroupBy(g => new { g.Year, g.Quarter })
                   .Select(i => new Models.Turnover.ChartItem()
                   {
                       Turnover = (0.5d * (i.Sum(g => g.Hires) + i.Sum(g => g.Separations)) / i.Sum(g => g.Employment)) * 100,
                       Hires = i.Sum(g => g.Hires),
                       Separations = i.Sum(g => g.Separations),
                       Name = "USA"
                   });

                //state
                var state = context.LaborDynamicsByStates.Where(i =>
                    i.NAICSId == naics4.FirstOrDefault().Id &&
                    i.StateId == locations.FirstOrDefault().State.Id);
                var sMax = state.Select(i => new { i.Year, i.Quarter })
                    .OrderByDescending(i => i.Year)
                    .ThenByDescending(i => i.Quarter);
                var stateData = state
                    .Where(i =>
                        i.Year == sMax.FirstOrDefault().Year &&
                        i.Quarter == sMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Turnover = i.Turnover * 100,
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Name = locations.FirstOrDefault().State.Name
                    });

                //metro
                var metro = context.LaborDynamicsByMetroes.Where(i =>
                    i.NAICSId == naics4.FirstOrDefault().Id &&
                    i.MetroId == locations.FirstOrDefault().Metro.Id);
                var mMax = metro.Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);
                var metroData = metro.Where(i =>
                     i.Year == mMax.FirstOrDefault().Year &&
                        i.Quarter == mMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Turnover = i.Turnover * 100,
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Name = locations.FirstOrDefault().Metro.Name
                    });


                //county
                var county = context.LaborDynamicsByCounties.Where(i =>
                    i.NAICSId == naics4.FirstOrDefault().Id &&
                    i.CountyId == locations.FirstOrDefault().County.Id);
                var cMax = county.Select(i => new { i.Year, i.Quarter })
                  .OrderByDescending(i => i.Year)
                  .ThenByDescending(i => i.Quarter);
                var countyData = county
                    .Where(i =>
                        i.Year == cMax.FirstOrDefault().Year &&
                        i.Quarter == cMax.FirstOrDefault().Quarter)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Turnover = i.Turnover * 100,
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Name = locations.FirstOrDefault().County.Name + ", " + locations.FirstOrDefault().State.Abbreviation
                    });




                var data = countyData.Select(i => new Models.Charts.BarChart()
                {
                    Nation = nationData.FirstOrDefault(),
                    State = stateData.FirstOrDefault(),
                    Metro = metroData.FirstOrDefault(),
                    County = i
                }).FirstOrDefault();


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(int industryId, int countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var naics4 = context.Industries.Where(i => i.Id == industryId)
                   .Select(i => context.NAICS.Where(n => n.NAICSCode == context.SicToNAICSMappings
                           .Where(m => m.IndustryId == i.Id)
                           .Select(m => m.NAICS)
                           .FirstOrDefault()
                           .NAICSCode.Substring(0, 4)
                      ).FirstOrDefault()
                   );


                var turnovers = context.LaborDynamicsByCounties
                    .Where(i =>
                        i.NAICSId == naics4.FirstOrDefault().Id
                       )
                    .Select(i => new { i.Turnover, i.CountyId, i.Year, i.Quarter });

                var maxes = turnovers
                   .Select(i => new { i.Year, i.Quarter })
                   .OrderByDescending(i => i.Year)
                   .ThenByDescending(i => i.Quarter);

                turnovers = turnovers.Where(i =>
                    i.Year == maxes.FirstOrDefault().Year &&
                        i.Quarter == maxes.FirstOrDefault().Quarter
                        );

                var data = new
                {
                    Total = turnovers.Count(),
                    Less = turnovers.Where(i => i.Turnover < turnovers.Where(d => d.CountyId == countyId).Select(d => d.Turnover).FirstOrDefault()).Count()
                };

                object obj = null;
                if (data.Total > 0)
                {
                    obj = new
                    {
                        Percentile = (int)(((decimal)data.Less / (decimal)data.Total) * 100)
                    };
                }

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
