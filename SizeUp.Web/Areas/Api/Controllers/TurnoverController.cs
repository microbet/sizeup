using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;


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
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Turnover = i.TurnoverRate,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Turnover = i.TurnoverRate,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.Turnover.ChartItem()
                    {
                        Hires = i.Hires,
                        Separations = i.Separations,
                        Turnover = i.TurnoverRate,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.Turnover.ChartItem()
                   {
                       Hires = i.Hires,
                       Separations = i.Separations,
                       Turnover = i.TurnoverRate,
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

        public ActionResult Percentile(int industryId, int countyId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var turnovers = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.TurnoverRate);

                var currentTurnover = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.CountyId == countyId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.TurnoverRate)
                    .FirstOrDefault();

                var data = new
                {
                    Total = turnovers.Count(),
                    Less = turnovers.Where(i => i.Value < currentTurnover).Count()
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
