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
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class YearStartedController : BaseController
    {
        //
        // GET: /Api/YearStarted/
        [APIRequest]
        public ActionResult Chart(long industryId, long placeId, int startYear, int endYear, Granularity granularity)
        {
            if (startYear < 1986)
            {
                startYear = 1986;
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [APIRequest]
        public ActionResult Percentile(long industryId, long placeId, int value, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Percentile(context, industryId, placeId, value, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
