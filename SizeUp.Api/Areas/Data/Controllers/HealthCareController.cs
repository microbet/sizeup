using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class HealthCareController : BaseController
    {
        //
        // GET: /Api/HealthCare/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long placeId, long? employees, Core.DataLayer.Granularity granularity = Core.DataLayer.Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Healthcare.Chart(context, industryId, placeId, employees, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentage(int industryId, long placeId, long value, Core.DataLayer.Granularity granularity = Core.DataLayer.Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.Healthcare.Percentage(context, industryId, placeId, value, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
