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
    public class JobChangeController : Controller
    {
        //
        // GET: /Api/JobChange/

        public ActionResult JobChange(int industryId, int countyId)
        {


            var naics4 = DataContexts.SizeUpContext.Industries.Where(i => i.Id == industryId)
               .Select(i => DataContexts.SizeUpContext.NAICS.Where(n => n.NAICSCode == DataContexts.SizeUpContext.SicToNAICSMappings
                       .Where(m => m.IndustryId == i.Id)
                       .Select(m => m.NAICS)
                       .FirstOrDefault()
                       .NAICSCode.Substring(0, 4)
                  ).FirstOrDefault()
               );

            var locations = DataContexts.SizeUpContext.Counties
                .Select(i => new
                {
                    County = i,
                    Metro = i.Metro,
                    State = i.State
                })
                .Where(i => i.County.Id == countyId);



            //nation
            var nation = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i => i.NAICSId == naics4.FirstOrDefault().Id);
            var nMax = nation.Select(i => new { i.Year, i.Quarter })
                .OrderByDescending(i => i.Year)
                .ThenByDescending(i => i.Quarter);
            var nationData = nation.Where(i =>
                i.Year == nMax.FirstOrDefault().Year &&
                i.Quarter == nMax.FirstOrDefault().Quarter)

               .GroupBy(g => new { g.Year, g.Quarter })
               .Select(i => new Models.JobChange.ChartItem()
               {
                   NetJobChange = i.Sum(g => g.NetJobChange),
                   JobGains = i.Sum(g => g.JobGains),
                   JobLosses = i.Sum(g => g.JobLosses),
                   Name = "USA"
               });

            //state
            var state = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i =>
                i.NAICSId == naics4.FirstOrDefault().Id &&
                i.StateId == locations.FirstOrDefault().State.Id);
            var sMax = state.Select(i => new { i.Year, i.Quarter })
                .OrderByDescending(i => i.Year)
                .ThenByDescending(i => i.Quarter);
            var stateData = state
                .Where(i =>
                    i.Year == sMax.FirstOrDefault().Year &&
                    i.Quarter == sMax.FirstOrDefault().Quarter)
                .Select(i => new Models.JobChange.ChartItem()
                {
                    NetJobChange = i.NetJobChange,
                    JobGains = i.JobGains,
                    JobLosses = i.JobLosses,
                    Name = locations.FirstOrDefault().State.Name
                });

            //metro
            var metro = DataContexts.SizeUpContext.LaborDynamicsByMetroes.Where(i =>
                i.NAICSId == naics4.FirstOrDefault().Id &&
                i.MetroId == locations.FirstOrDefault().Metro.Id);
            var mMax = metro.Select(i => new { i.Year, i.Quarter })
               .OrderByDescending(i => i.Year)
               .ThenByDescending(i => i.Quarter);
            var metroData = metro.Where(i =>
                 i.Year == mMax.FirstOrDefault().Year &&
                    i.Quarter == mMax.FirstOrDefault().Quarter)
                .Select(i => new Models.JobChange.ChartItem()
                {
                    NetJobChange = i.NetJobChange,
                    JobGains = i.JobGains,
                    JobLosses = i.JobLosses,
                    Name = locations.FirstOrDefault().Metro.Name
                });


            //county
            var county = DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(i =>
                i.NAICSId == naics4.FirstOrDefault().Id &&
                i.CountyId == locations.FirstOrDefault().County.Id);
            var cMax = county.Select(i => new { i.Year, i.Quarter })
              .OrderByDescending(i => i.Year)
              .ThenByDescending(i => i.Quarter);
            var countyData = county
                .Where(i =>
                    i.Year == cMax.FirstOrDefault().Year &&
                    i.Quarter == cMax.FirstOrDefault().Quarter)
                .Select(i => new Models.JobChange.ChartItem()
                {
                    NetJobChange = i.NetJobChange,
                    JobGains = i.JobGains,
                    JobLosses = i.JobLosses,
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
}
