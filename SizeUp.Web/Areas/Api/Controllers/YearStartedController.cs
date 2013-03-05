﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class YearStartedController : BaseController
    {
        //
        // GET: /Api/YearStarted/

        public ActionResult YearStarted(long industryId, long placeId, int startYear, int endYear, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Chart(context, industryId, placeId, startYear, endYear, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

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
