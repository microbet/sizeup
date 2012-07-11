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
    public class RevenuePerCapitaController : Controller
    {
        //
        // GET: /Api/RevenuePerCapita/

        public ActionResult RevenuePerCapita(int industryId, int countyId)
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
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.ReveuePerCapita,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.ReveuePerCapita,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.RevenuePerCapita.ChartItem()
                    {
                        Value = i.ReveuePerCapita,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.RevenuePerCapita.ChartItem()
                   {
                       Value = i.ReveuePerCapita,
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

                var data = context.IndustryDataByCounties
                    .Where(i => i.IndustryId == industryId && i.CountyId == countyId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.ReveuePerCapita)
                    .FirstOrDefault();

                object obj = null;
                if (data != null && value != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - data) / data) * 100)
                    };
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
