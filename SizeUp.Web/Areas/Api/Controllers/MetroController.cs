using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using Microsoft.SqlServer.Types;
using System.Data.Objects;
using System.Data.Spatial;
namespace SizeUp.Web.Areas.Api.Controllers
{
    public class MetroController : BaseController
    {
        //
        // GET: /Api/Metro/

        public JsonResult Metro(int? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Metroes.Where(i => i.Id == id);
                var data = item.Select(i => new Models.Metro.Metro()
                {
                    Id = i.Id,
                    Name = i.Name
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BoundingBox(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.MetroGeographies.Where(i => i.MetroId == id && i.GeographyClass.Name == "Calculation");
                var data = item.Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                List<Models.Maps.LatLng> output = new List<Models.Maps.LatLng>();
                if (data != null)
                {
                    var geom = DbGeometry.FromBinary(data.AsBinary());
                    geom = geom.Envelope;
                    var geo = DbGeography.FromBinary(geom.AsBinary());
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.PointAt(1).Latitude, Lng = (double)geo.PointAt(1).Longitude });
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.PointAt(3).Latitude, Lng = (double)geo.PointAt(3).Longitude });
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Centroid(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.MetroGeographies.Where(i => i.MetroId == id && i.GeographyClass.Name == "Calculation");
                var data = item.Select(i => i.Geography.GeographyPolygon).FirstOrDefault();
                Models.Maps.LatLng output = new Models.Maps.LatLng();
                if (data != null)
                {
                    var geom = DbGeometry.FromBinary(data.AsBinary());
                    geom = geom.ConvexHull.Centroid;
                    var geo = DbGeography.FromBinary(geom.AsBinary());
                    output.Lat = (double)geo.Latitude;
                    output.Lng = (double)geo.Longitude;
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
