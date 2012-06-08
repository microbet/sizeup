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
                var data = item.Select(i => new Models.City.City()
                {
                    Id = i.Id,
                    Name = i.Name,
                    County = i.County.Name,
                    State = i.State.Abbreviation,
                    SEOKey = i.SEOKey
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CurrentCity()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Cities
                    .Where(i => i.Id == SizeUp.Core.Web.WebContext.Current.CurrentCityId)
                    .Select(i => new Models.City.City()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        County = i.County.Name,
                        State = i.State.Abbreviation,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CurrentCity(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = context.Cities.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentCityId = id;
                }
                return Json(c != null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DetectedCity()
        {
            var id = GeoCoder.GetCityIdByIPAddress();
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Cities
                    .Where(i => i.Id == id)
                    .Select(i => new Models.City.City()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        County = i.County.Name,
                        State = i.State.Abbreviation,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchCities(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var keywords = context.Cities.AsQueryable();
                var data = keywords.Select(i => new Models.City.City()
                {
                    Id = i.Id,
                    Name = i.Name,
                    County = i.County.Name,
                    State = i.State.Abbreviation,
                    SEOKey = i.SEOKey
                });

                data = data.Where(i => (i.Name + " " + i.State).StartsWith(term));
                data = data.OrderBy(i => i.Name);
                data = data.Take(maxResults);
                var list = data.ToList();

                list.ForEach(i => i.DisplayName = list.Count(s => s.Name == i.Name && s.State == i.State) > 1 ? string.Format("{0}, {1} ({2})", i.Name, i.State, i.County) : string.Format("{0}, {1}", i.Name, i.State));

                return Json(list, JsonRequestBehavior.AllowGet);
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
