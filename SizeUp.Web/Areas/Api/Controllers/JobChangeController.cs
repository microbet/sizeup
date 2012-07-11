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
    public class JobChangeController : Controller
    {
        //
        // GET: /Api/JobChange/

        public ActionResult JobChange(int industryId, int countyId)
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
                    .Select(i => new Models.JobChange.ChartItem()
                    {
                        NetJobChange = i.NetJobChange,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        Name = "USA"
                    });


                var s = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.JobChange.ChartItem()
                    {
                        NetJobChange = i.NetJobChange,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        Name = locations.State.Name
                    });

                var m = context.IndustryDataByMetroes
                    .Where(i => i.IndustryId == industryId && i.MetroId == locations.Metro.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.JobChange.ChartItem()
                    {
                        NetJobChange = i.NetJobChange,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        Name = locations.Metro.Name
                    });

                var co = context.IndustryDataByCounties
                   .Where(i => i.IndustryId == industryId && i.CountyId == locations.County.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.JobChange.ChartItem()
                   {
                       NetJobChange = i.NetJobChange,
                       JobGains = i.JobGains,
                       JobLosses = i.JobLosses,
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



    }
}
