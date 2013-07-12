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
using SizeUp.Api.Controllers;
using SizeUp.Core.API;

namespace SizeUp.Api.Areas.Data.Controllers
{
    public class GeographyController : BaseController
    {
        //
        // GET: /Api/Geography/
        
        [APIAuthorize(Role = "Place")]
        public ActionResult Centroid(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Get(context)
                    .Where(i => i.GeographicLocationId == id)
                    .Where(new Core.DataLayer.Filters.Geography.Calculation().Expression)
                    .Select(new Core.DataLayer.Projections.Geography.Centroid().Expression)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "Place")]
        public ActionResult BoundingBox(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Get(context)
                    .Where(i => i.GeographicLocationId == id)
                    .Where(new Core.DataLayer.Filters.Geography.Calculation().Expression)
                    .Select(new Core.DataLayer.Projections.Geography.BoundingBox().Expression)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "Place")]
        public ActionResult ZoomExtent(long id, long width)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context)
                    .Where(i => i.Id == id)
                    .Select(new Core.DataLayer.Projections.Geography.ZoomExtent(width).Expression)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
