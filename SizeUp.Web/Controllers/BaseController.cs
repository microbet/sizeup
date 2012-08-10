using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Serialization;
using Api = SizeUp.Web.Areas.Api;
namespace SizeUp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Models.CurrentInfo CurrentInfo { get; set; }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var data = new Models.Header();

            
            using (var context = ContextFactory.SizeUpContext)
            {
                if (!string.IsNullOrEmpty((string)requestContext.RouteData.Values["city"]) && !string.IsNullOrEmpty((string)requestContext.RouteData.Values["county"]) && !string.IsNullOrEmpty((string)requestContext.RouteData.Values["state"]))
                {
                    var city = (string)requestContext.RouteData.Values["city"];
                    var county = (string)requestContext.RouteData.Values["county"];
                    var state = (string)requestContext.RouteData.Values["state"];
                    WebContext.Current.CurrentPlaceId = context.CityCountyMappings
                        .Where(i => i.County.SEOKey == county && i.City.State.SEOKey == state && i.City.SEOKey == city)
                        .Select(i => i.Id)
                        .FirstOrDefault();
                }

                if (!string.IsNullOrEmpty((string)requestContext.RouteData.Values["industry"]))
                {
                    var industry = (string)requestContext.RouteData.Values["industry"];
                    WebContext.Current.CurrentIndustryId = context.Industries.Where(i => i.SEOKey == industry).Select(i => i.Id).FirstOrDefault();
                }

                CurrentInfo = new Models.CurrentInfo()
                {
                    CurrentPlace = context.CityCountyMappings.Where(i => i.Id == WebContext.Current.CurrentPlaceId).Select(i => new Api.Models.Place.Place()
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
                    }).FirstOrDefault(),

                    CurrentIndustry = context.Industries.Where(i => i.Id == WebContext.Current.CurrentIndustryId).Select(i => new Api.Models.Industry.Industry()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault()
                };

                ViewBag.CurrentInfo = CurrentInfo;
                ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
            }

            ViewBag.Header = data;
           


        }
    }
}
