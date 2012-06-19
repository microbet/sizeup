using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Web.Areas.Api.Models.City;
using Microsoft.SqlServer.Types;
using System.Data.Objects;
namespace SizeUp.Web.Areas.Api.Controllers
{
    public class CityController : Controller
    {
        //
        // GET: /Api/City/

        public JsonResult City(int? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Cities.Where(i => i.Id == id);
                var data = item.Select(i =>  new Api.Models.City.City()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey,
                        State = i.State.Abbreviation
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BoundingBox(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Cities.Where(i => i.Id == id);
                var data = item.Select(i => i.Geography).FirstOrDefault();
                List<Models.Maps.LatLng> output = new List<Models.Maps.LatLng>();
                if (data != null)
                {
                    var geo = SqlGeography.Parse(data.AsText());
                    var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                    geom = geom.STEnvelope();
                    geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.STPointN(1).Lat, Lng = (double)geo.STPointN(1).Long });
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.STPointN(3).Lat, Lng = (double)geo.STPointN(3).Long });
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Centroid(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var item = context.Cities.Where(i => i.Id == id);
                var data = item.Select(i => i.Geography).FirstOrDefault();
                Models.Maps.LatLng output = new Models.Maps.LatLng();
                if (data != null)
                {
                    var geo = SqlGeography.Parse(data.AsText());
                    var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
                    geom = geom.STCentroid();
                    geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
                    output.Lat = (double)geo.STPointN(1).Lat;
                    output.Lng = (double)geo.STPointN(1).Long;
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
