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
               ).FirstOrDefault();

            var locations = DataContexts.SizeUpContext.Counties.Where(i => i.Id == countyId)
                .Select(i => new
                {
                    County = i,
                    Metro = i.Metro,
                    State = i.State
                })
                .FirstOrDefault();




            Api.Models.JobChange.JobChangeDynamics nation = null;
            Api.Models.JobChange.JobChangeDynamics state = null;
            Api.Models.JobChange.JobChangeDynamics metro = null;
            Api.Models.JobChange.JobChangeDynamics county = null;



            nation = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByStates.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByStates.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id)
                .GroupBy(g=> new {g.Year, g.Quarter})
                .Select(i => new Api.Models.JobChange.JobChangeDynamics() { NetJobChange = i.Sum(g=>g.NetJobChange), JobGains = i.Sum(g=>g.JobGains), JobLosses = i.Sum(g=>g.JobLosses) })
                .FirstOrDefault();

            state = DataContexts.SizeUpContext.LaborDynamicsByStates.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByStates.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByStates.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id && i.StateId == locations.State.Id)
                .Select(i => new Api.Models.JobChange.JobChangeDynamics() { NetJobChange = i.NetJobChange, JobGains = i.JobGains, JobLosses = i.JobLosses })
                .FirstOrDefault();

            if (locations.Metro != null)
            {
                metro = DataContexts.SizeUpContext.LaborDynamicsByMetroes.Where(i =>
                    i.Year == DataContexts.SizeUpContext.LaborDynamicsByMetroes.Max(m => m.Year) &&
                    i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByMetroes.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                    i.NAICSId == naics4.Id && i.MetroId == locations.Metro.Id)
                    .Select(i => new Api.Models.JobChange.JobChangeDynamics() { NetJobChange = i.NetJobChange, JobGains = i.JobGains, JobLosses = i.JobLosses })
                    .FirstOrDefault();
            }

            county = DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(i =>
                i.Year == DataContexts.SizeUpContext.LaborDynamicsByCounties.Max(m => m.Year) &&
                i.Quarter == DataContexts.SizeUpContext.LaborDynamicsByCounties.Where(q => q.Year == i.Year).Max(m => m.Quarter) &&
                i.NAICSId == naics4.Id && i.CountyId == locations.County.Id)
                .Select(i => new Api.Models.JobChange.JobChangeDynamics() { NetJobChange = i.NetJobChange, JobGains = i.JobGains, JobLosses = i.JobLosses })
                .FirstOrDefault();


            var obj = new Models.Charts.BarChart();
            if (nation != null)
            {
                obj.Nation = new Models.JobChange.ChartItem()
                {
                    JobGains = nation.JobGains,
                    JobLosses = nation.JobLosses,
                    NetJobChange = nation.NetJobChange,
                    Name = "USA"
                };
            }
            if (state != null)
            {
                obj.State = new Models.JobChange.ChartItem()
                {
                    JobGains = state.JobGains,
                    JobLosses = state.JobLosses,
                    NetJobChange = state.NetJobChange,
                    Name = locations.State.Name
                };
            }
            if (locations.Metro != null && metro != null)
            {
                obj.Metro = new Models.JobChange.ChartItem()
                {
                    JobGains = metro.JobGains,
                    JobLosses = metro.JobLosses,
                    NetJobChange = metro.NetJobChange,
                    Name = locations.Metro.Name
                };
            }
            if (county != null)
            {
                obj.County = new Models.JobChange.ChartItem()
                {
                    JobGains = county.JobGains,
                    JobLosses = county.JobLosses,
                    NetJobChange = county.NetJobChange,
                    Name = string.Format("{0}, {1}", locations.County.Name, locations.State.Abbreviation)
                };
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }



    }
}
