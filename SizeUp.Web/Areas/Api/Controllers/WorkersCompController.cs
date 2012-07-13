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
    public class WorkersCompController : Controller
    {
        //
        // GET: /Api/WorkersComp/

        public ActionResult WorkersComp(long industryId, long cityId, int? employees)
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
                   .Where(i => i.City.Id ==cityId).FirstOrDefault();


                var data = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => new Models.WorkersComp.ChartItem()
                    {
                        Rank = 1,
                        Average = 2.12f
                    });

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, int cityId, decimal value)
        {
            //TODO need to fix this once we get the data in the database
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



                var salary = context.IndustryDataByStates
                    .Where(i => i.IndustryId == industryId && i.StateId == locations.State.Id && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                    .Select(i => i.Quarter)
                    .FirstOrDefault();

                object obj = null;
                if (salary != null && salary != 0)
                {
                    obj = new
                    {
                        Percentage = (int)(((value - salary) / salary) * 100)
                    };
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
