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
    public class PlaceController : Controller
    {
        //
        // GET: /Api/Place/

        public JsonResult SearchPlaces(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {

                var data = context.CityCountyMappings
                .Where(i => i.City.Name.StartsWith(term))
                .OrderBy(i => i.City.Name)
                .ThenBy(i=>i.City.State.Abbreviation)
                .Take(maxResults)
                .Select(i => new Api.Models.Place.Place()
                {
                    Id = i.Id,
                    City = new Api.Models.City.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        State = i.City.State.Abbreviation
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

                data.ForEach(i => i.DisplayName = data.Count(s => s.City.Name == i.City.Name && s.City.State == i.City.State) > 1 ? string.Format("{0}, {1} ({2})", i.City.Name, i.City.State, i.County.Name) : string.Format("{0}, {1}", i.City.Name, i.City.State));

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CurrentPlace()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.CityCountyMappings
                    .Where(i => i.Id == SizeUp.Core.Web.WebContext.Current.CurrentCityId)
                    .Select(i => new Api.Models.Place.Place()
                    {
                        Id = i.Id,
                        City = new Api.Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            State = i.City.State.Abbreviation
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
                    WebContext.Current.CurrentCityId = id;
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
                            State = i.City.State.Abbreviation
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
                            State = i.City.State.Abbreviation
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


    }
}
