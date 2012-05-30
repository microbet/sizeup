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

        public ActionResult AverageSalary(int industryId, int countyId)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);

            var locations = DataContexts.SizeUpContext.Counties
                .Select(i => new
                {
                    County = i,
                    Metro = i.Metro,
                    State = i.State
                })
                .Where(i => i.County.Id == countyId);

               
            var nation = DataContexts.SizeUpContext.AverageSalaryNationals
                .Where(i =>
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.AverageSalary > 0);
            var nMax = nation.Select(i => i.Year)
                .OrderByDescending(i => i);
            var nationData = nation.Where(i =>
                i.Year == nMax.FirstOrDefault())
                .Select(i => new Models.AverageSalary.ChartItem()
                {
                    Value = i.AverageSalary,
                    Name = "USA"
                });


            
            
            var state = DataContexts.SizeUpContext.AverageSalaryByStates
                .Where(i => 
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.StateId == locations.FirstOrDefault().State.Id &&
                    i.AverageSalary > 0);
            var sMax = state.Select(i => i.Year)
                .OrderByDescending(i => i);
            var stateData = state.Where(i =>
                i.Year == sMax.FirstOrDefault())
                .Select(i => new Models.AverageSalary.ChartItem()
                {
                    Value = i.AverageSalary,
                    Name = locations.FirstOrDefault().State.Name
                });
     
            var metro = DataContexts.SizeUpContext.AverageSalaryByMetroes
                .Where(i => 
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.MetroId == locations.FirstOrDefault().Metro.Id &&
                    i.AverageSalary > 0);
            var mMax = metro.Select(i => i.Year)
                .OrderByDescending(i => i);
            var metroData = metro.Where(i=>
                i.Year == mMax.FirstOrDefault())
                .Select(i => new Models.AverageSalary.ChartItem()
                {
                    Value = i.AverageSalary,
                    Name = locations.FirstOrDefault().Metro.Name
                });
         

            var county = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i =>
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.CountyId == locations.FirstOrDefault().County.Id &&
                    i.AverageSalary > 0);
            var cMax = county.Select(i => i.Year)
                .OrderByDescending(i => i);
            var countyData = county.Where(i=>
                i.Year == cMax.FirstOrDefault())
                .Select(i => new Models.AverageSalary.ChartItem()
                {
                    Value = i.AverageSalary,
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

        public ActionResult Percentage(int industryId, int countyId, decimal value)
        {
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);
            var countyData = DataContexts.SizeUpContext.AverageSalaryByCounties.Where(i =>
                i.NAICSId == naics.FirstOrDefault().Id &&
                i.CountyId == countyId);

            var cMax = countyData.Select(i => i.Year)
               .OrderByDescending(i => i);

            long? county = countyData.Where(i => i.Year == cMax.FirstOrDefault())
                .Select(i => i.AverageSalary).FirstOrDefault();
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


        public ActionResult BandsByCounty(int industryId, int bands, string boundingEntityId)
        {
            BoundingEntity boundingEntity = new BoundingEntity(boundingEntityId);
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);
            var filters = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i => 
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.AverageSalary > 0);

            var cMax = filters.Select(i => i.Year)
               .OrderByDescending(i => i);

            filters = filters.Where(i => i.Year == cMax.FirstOrDefault());

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
                .Select(b => new Models.AverageSalary.Band(){ Min = b.Min(i => i.AverageSalary), Max = b.Max(i => i.AverageSalary) })
                .ToList();

            Models.AverageSalary.Band old = null;
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
            var naics = DataContexts.SizeUpContext.SicToNAICSMappings.Where(i => i.IndustryId == industryId).Select(i => i.NAICS);
            var filters = DataContexts.SizeUpContext.AverageSalaryByCounties
                .Where(i => 
                    i.NAICSId == naics.FirstOrDefault().Id &&
                    i.AverageSalary > 0);

            var sMax = filters.Select(i => i.Year)
               .OrderByDescending(i => i);

            filters = filters.Where(i => i.Year == sMax.FirstOrDefault());

            var data = filters.Select(i => new { i.AverageSalary, i.CountyId }).ToList();
            var bandData = data.NTile(i => i.AverageSalary, bands)
                .Select(b => new Models.AverageSalary.Band() { Min = b.Min(i => i.AverageSalary), Max = b.Max(i => i.AverageSalary) })
                .ToList();

            Models.AverageSalary.Band old = null;
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
