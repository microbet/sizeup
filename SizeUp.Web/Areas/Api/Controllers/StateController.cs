using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;

using Microsoft.SqlServer.Types;
using System.Data.Spatial;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class StateController : Controller
    {
        //
        // GET: /Api/Geography/

        public ActionResult Polygon()
        {
            string idsRaw = Request["ids"];
            var ids = idsRaw.Split(',').Select(i => long.Parse(i));
            var data = DataContexts.SizeUpContext.States
                .Where(i => ids.Contains(i.Id))
                .ToList()
                .Select(i => new { Polygon = Models.Maps.Polygon.Create(SqlGeography.Parse(i.Geography.AsText()), 0), Id = i.Id })
                .ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            ContentResult result = new ContentResult();
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult Contained(string ne, string sw, int buffer = 0)
        {
            Models.Maps.LatLng swP = Models.Maps.LatLng.FromText(sw);
            Models.Maps.LatLng neP = Models.Maps.LatLng.FromText(ne);
            var bounds = DbGeography.FromText(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", swP.Lng, neP.Lng, swP.Lat, neP.Lat));
            bounds = bounds.Buffer(buffer);

            var data = DataContexts.SizeUpContext.States
                .Where(i => i.Geography.Intersects(bounds))
                .Select(i => i.Id)
                .ToList();
               
            return Json(data, JsonRequestBehavior.AllowGet);
        }



    }
}
