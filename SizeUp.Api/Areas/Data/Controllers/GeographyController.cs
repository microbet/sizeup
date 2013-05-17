﻿using System;
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
using SizeUp.Api.Controllers;
using SizeUp.Core.API;

namespace SizeUp.Api.Areas.Data.Controllers
{
    public class GeographyController : BaseController
    {
        //
        // GET: /Api/Geography/
        [APIRequest]
        public ActionResult Centroid(long id, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Centroid(context, granularity).Where(i => i.Key == id).Select(i=>i.Value).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIRequest]
        public ActionResult BoundingBox(long id, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.BoundingBox(context, granularity).Where(i => i.Key == id).Select(i => i.Value).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIRequest]
        public ActionResult ZoomExtent(long id, long width)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.ZoomExtent(context, width).Where(i => i.PlaceId == id).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}