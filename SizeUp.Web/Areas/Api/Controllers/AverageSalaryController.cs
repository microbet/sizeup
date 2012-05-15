using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
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

            national = DataContexts.SizeUpContext.AverageSalaryNationals.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryNationals.Max(m => m.Year) && i.NAICSId == naics.Id).Select(i => i.AverageSalary).FirstOrDefault();
            state = DataContexts.SizeUpContext.AverageSalaryByStates.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByStates.Max(m => m.Year) && i.NAICSId == naics.Id && i.StateId == locations.State.Id).Select(i => i.AverageSalary).FirstOrDefault();
            if (locations.Metro != null)
            {
                metro = DataContexts.SizeUpContext.AverageSalaryByMetroes.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByMetroes.Max(m => m.Year) && i.NAICSId == naics.Id && i.MetroId == locations.Metro.Id).Select(i => i.AverageSalary).FirstOrDefault();
            }
            county = DataContexts.SizeUpContext.AverageSalaryByCounties.Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id && i.CountyId == locations.County.Id).Select(i => i.AverageSalary).FirstOrDefault();

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
            if (locations.Metro != null)
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


        public ActionResult BandsByCounty(int industryId, int bands = 1)
        {


            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            var filters = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByCounties.Max(m => m.Year) && i.NAICSId == naics.Id);
                
                

            if (!string.IsNullOrEmpty(Request["stateId"]))
            {
                long stateId = long.Parse(Request["stateId"]);
                filters = filters.Where(i => i.County.StateId == stateId);
            }
            var data = filters.OrderBy(i => i.AverageSalary)
                .Select(i => new { value = i.AverageSalary, id = i.CountyId })
                .ToList();

            long min = data.Min(i => i.value).Value;
            long max = data.Max(i => i.value).Value;
            int count = data.Count;

            int itemsPerBand = (int)Math.Ceiling((decimal)count / bands);

            List<List<object>> groups = new List<List<object>>();
            for (int x = 0; x < bands; x++)
            {
                List<object> g = new List<object>();
                var items = data.Skip(itemsPerBand * x).Take(itemsPerBand).ToList();
                if (items.Count > 0)
                {
                    foreach (var i in items)
                    {
                        g.Add(i);
                    }
                    groups.Add(g);
                }
            }
            return Json(groups, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BandsByState(int industryId, int bands = 1)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS).FirstOrDefault();
            var data = DataContexts.SizeUpContext.AverageSalaryByStates
                .Where(i => i.Year == DataContexts.SizeUpContext.AverageSalaryByStates.Max(m => m.Year) && i.NAICSId == naics.Id)
                .OrderBy(i => i.AverageSalary)
                .Select(i => new { value =  i.AverageSalary, id = i.StateId })
                .ToList();

            long min = data.Min(i => i.value).Value;
            long max = data.Max(i => i.value).Value;
            int count = data.Count;

            int itemsPerBand = (int)Math.Ceiling((decimal)count / bands);

            List<List<object>> groups = new List<List<object>>();
            for (int x = 0; x < bands; x++)
            {
                List<object> g = new List<object>();
                var items = data.Skip(itemsPerBand * x).Take(itemsPerBand).ToList();
                if (items.Count > 0)
                {
                    foreach (var i in items)
                    {
                        g.Add(i);
                    }
                    groups.Add(g);
                }
            }
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

    }
}
