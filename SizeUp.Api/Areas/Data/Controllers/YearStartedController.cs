using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class YearStartedController : BaseController
    {
        //
        // GET: /Api/YearStarted/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long geographicLocationId, int startYear, int endYear)
        {
            if (startYear < 1986)
            {
                startYear = 1986;
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Chart(context, industryId, geographicLocationId, startYear, endYear);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, int value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Percentile(context, industryId, geographicLocationId, value);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
