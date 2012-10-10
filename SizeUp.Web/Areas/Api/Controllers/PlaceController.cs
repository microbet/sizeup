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
using System.Data.Spatial;
using SizeUp.Core;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class PlaceController : BaseController
    {
        //
        // GET: /Api/Place/

        public JsonResult SearchPlaces(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var data = context.CityCountyMappings
                .Where(i => i.City.Name.StartsWith(term) && i.City.CityType.IsActive)
                .OrderBy(i => i.City.Name)
                .ThenBy(i=>i.City.State.Abbreviation)
                .ThenByDescending(i=>i.City.DemographicsByCities.Where(d=>d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault().TotalPopulation)
                .Take(maxResults)
                .Select(i => new Api.Models.Place.Place()
                {
                    Id = i.Id,
                    City = new Api.Models.City.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        State = i.City.State.Abbreviation,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Api.Models.County.County()
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey,
                        State = i.County.State.Abbreviation
                    },
                    Metro = new Api.Models.Metro.Metro()
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name
                    },
                    State = new Api.Models.State.State()
                    {
                        Id = i.County.State.Id,
                        Name = i.County.State.Name,
                        Abbreviation = i.County.State.Abbreviation,
                        SEOKey = i.County.State.SEOKey
                    }
                }).ToList();

                data.ForEach(i => i.DisplayName = data.Count(s => s.City.Name == i.City.Name && s.City.State == i.City.State) > 1 ? string.Format("{0}, {1} ({2} County - {3})", i.City.Name, i.City.State, i.County.Name, i.City.TypeName) : string.Format("{0}, {1}", i.City.Name, i.City.State));

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CurrentPlace()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings
                    .Where(i => i.Id == SizeUp.Core.Web.WebContext.Current.CurrentPlaceId)
                    .Select(i => new Api.Models.Place.Place()
                    {
                        Id = i.Id,
                        City = new Api.Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            State = i.City.State.Abbreviation,
                            TypeName = i.City.CityType.Name
                        },
                        County = new Api.Models.County.County()
                        {
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey,
                            State = i.County.State.Abbreviation
                        },
                        Metro = new Api.Models.Metro.Metro()
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name
                        },
                        State = new Api.Models.State.State()
                        {
                            Id = i.County.State.Id,
                            Name = i.County.State.Name,
                            Abbreviation = i.County.State.Abbreviation,
                            SEOKey = i.County.State.SEOKey
                        }
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CurrentPlace(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = context.CityCountyMappings.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentPlaceId = id;
                }
                return Json(c != null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DetectedPlace()
        {
            var id = GeoCoder.GetPlaceIdByIPAddress();
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings
                    .Where(i => i.Id == id)
                    .Select(i => new Api.Models.Place.Place()
                    {
                        Id = i.Id,
                        City = new Api.Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            State = i.City.State.Abbreviation,
                            TypeName = i.City.CityType.Name
                        },
                        County = new Api.Models.County.County()
                        {
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey,
                            State = i.County.State.Abbreviation
                        },
                        Metro = new Api.Models.Metro.Metro()
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name
                        },
                        State = new Api.Models.State.State()
                        {
                            Id = i.County.State.Id,
                            Name = i.County.State.Name,
                            Abbreviation = i.County.State.Abbreviation,
                            SEOKey = i.County.State.SEOKey
                        }
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Ip()
        {
           
                return Json(Request.UserHostAddress, JsonRequestBehavior.AllowGet);
        }
       

        public JsonResult Get(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings
                    .Where(i => i.Id == id)
                    .Select(i => new Api.Models.Place.Place()
                    {
                        Id = i.Id,
                        City = new Api.Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            State = i.City.State.Abbreviation,
                            TypeName = i.City.CityType.Name
                        },
                        County = new Api.Models.County.County()
                        {
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey,
                            State = i.County.State.Abbreviation
                        },
                        Metro = new Api.Models.Metro.Metro()
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name
                        },
                        State = new Api.Models.State.State()
                        {
                            Id = i.County.State.Id,
                            Name = i.County.State.Name,
                            Abbreviation = i.County.State.Abbreviation,
                            SEOKey = i.County.State.SEOKey
                        }
                    }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Centroid(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings.Where(i => i.Id == id)
                    .Select(i => new
                    {
                        City = i.City.CityGeographies.Where(g => g.CityId == i.CityId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                        County = i.County.CountyGeographies.Where(g => g.CountyId == i.CountyId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()
                    })
                    .FirstOrDefault();


                Models.Maps.LatLng output = new Models.Maps.LatLng();
                if (data != null)
                {
                    var geom = DbGeometry.FromBinary(data.City.Intersection(data.County).AsBinary());
                    geom = geom.ConvexHull.Centroid;
                    var geo = DbGeography.FromBinary(geom.AsBinary());
                    output.Lat = (double)geo.Latitude;
                    output.Lng = (double)geo.Longitude;
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BoundingBox(int id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings.Where(i => i.Id == id)
                    .Select(i => new
                    {
                        City = i.City.CityGeographies.Where(g => g.CityId == i.CityId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault(),
                        County = i.County.CountyGeographies.Where(g => g.CountyId == i.CountyId && g.GeographyClass.Name == "Calculation").Select(g => g.Geography.GeographyPolygon).FirstOrDefault()
                    })
                    .FirstOrDefault();

                List<Models.Maps.LatLng> output = new List<Models.Maps.LatLng>();
                if (data != null)
                {
                    var geom = DbGeometry.FromBinary(data.City.Intersection(data.County).AsBinary());
                    geom = geom.Envelope;
                    var geo = DbGeography.FromBinary(geom.AsBinary());
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.PointAt(1).Latitude, Lng = (double)geo.PointAt(1).Longitude });
                    output.Add(new Models.Maps.LatLng() { Lat = (double)geo.PointAt(3).Latitude, Lng = (double)geo.PointAt(3).Longitude });
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
