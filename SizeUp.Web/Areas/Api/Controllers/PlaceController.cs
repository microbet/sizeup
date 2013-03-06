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
                    .Where(i => i.City.CityType.IsActive);

                var search = term.Split(',');
                string city = search[0].Trim();
                string state = string.Empty;

                data = data.Where(i => i.City.Name.StartsWith(city) || i.PlaceKeywords.Any(cc => cc.Name.StartsWith(city)));
                if (search.Length > 1)
                {
                    state = search[1].Trim();
                    data = data.Where(i => i.County.State.Abbreviation.StartsWith(state));
                }

                data = data
                    .OrderBy(i => i.City.Name)
                    .ThenBy(i => i.City.State.Abbreviation)
                    .ThenByDescending(i => i.City.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault().TotalPopulation);
 
                var temp = data.Select(i => new Models.Place.Place()
                {
                    Id = i.Id,
                    DisplayName = context.CityCountyMappings.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        State = i.County.State.Abbreviation,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County.County()
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey,
                        State = i.County.State.Abbreviation
                    },
                    Metro = new Models.Metro.Metro()
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name
                    },
                    State = new Models.State.State()
                    {
                        Id = i.County.State.Id,
                        Name = i.County.State.Name,
                        Abbreviation = i.County.State.Abbreviation,
                        SEOKey = i.County.State.SEOKey
                    }
                });

                var dataOut = temp
                   .Take(maxResults).ToList();
            
                return Json(dataOut, JsonRequestBehavior.AllowGet);
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




    }
}
