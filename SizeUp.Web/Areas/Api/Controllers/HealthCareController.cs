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
    public class HealthCareController : Controller
    {
        //
        // GET: /Api/HealthCare/

        public ActionResult HealthCare(long industryId, long cityId, long? employees)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = context.CityCountyMappings
                    .Select(i => new
                    {
                        City = i.City,
                        County = i.County,
                        Metro = i.County.Metro,
                        State = i.County.State
                    })
                    .Where(i => i.City.Id == cityId).FirstOrDefault();



                var n = context.IndustryDataByStates
                   .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                   .Select(i => new Models.JobChange.ChartItem()
                   {
                       NetJobChange = i.NetJobChange,
                       JobGains = i.JobGains,
                       JobLosses = i.JobLosses,
                       Name = "USA"
                   });



                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
