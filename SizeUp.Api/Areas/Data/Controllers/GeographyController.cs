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
        public ActionResult Centroid(long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Get(context)
                    .Where(i => i.GeographicLocationId == geographicLocationId)
                    .Where(i => i.GeographyClass.Name == Core.Geo.GeographyClass.Calculation)
                    .Select(new Core.DataLayer.Projections.Geography.Centroid().Expression)
                    .Select(i => i.Value)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "Place")]
        public ActionResult BoundingBox(long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Get(context)
                    .Where(i => i.GeographicLocationId == geographicLocationId)
                    .Where(i => i.GeographyClass.Name == Core.Geo.GeographyClass.Calculation)
                    .Select(new Core.DataLayer.Projections.Geography.BoundingBox().Expression)
                    .Select(i=>i.Value)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "Place")]
        public ActionResult ZoomExtent(long placeId, long width)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.ZoomExtent(context, width)
                    .Where(i => i.PlaceId == placeId)
                    .FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
