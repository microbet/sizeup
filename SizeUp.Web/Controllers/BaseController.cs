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
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var data = new Models.Header();

            
            using (var context = ContextFactory.SizeUpContext)
            {
                if (!string.IsNullOrEmpty((string)requestContext.RouteData.Values["city"]) && !string.IsNullOrEmpty((string)requestContext.RouteData.Values["state"]))
                {
                    var city = (string)requestContext.RouteData.Values["city"];
                    var state = (string)requestContext.RouteData.Values["state"];
                    WebContext.Current.CurrentCityId = context.Cities.Where(i => i.SEOKey == city && i.State.Abbreviation == state).Select(i => i.Id).FirstOrDefault();
                    
                }

                if (!string.IsNullOrEmpty((string)requestContext.RouteData.Values["industry"]))
                {
                    var industry = (string)requestContext.RouteData.Values["industry"];
                    WebContext.Current.CurrentIndustryId = context.Industries.Where(i => i.SEOKey == industry).Select(i => i.Id).FirstOrDefault();
                }

                var info = new Models.CurrentInfo()
                {
                    CurrentCity = context.Cities.Where(i => i.Id == WebContext.Current.CurrentCityId).Select(i => new Api.Models.City.City()
                    {
                        Id = i.Id,
                        County = i.County.Name,
                        Name = i.Name,
                        State = i.State.Abbreviation,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault(),

                    CurrentIndustry = context.Industries.Where(i => i.Id == WebContext.Current.CurrentIndustryId).Select(i => new Api.Models.Industry.Industry()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    }).FirstOrDefault()
                };

                ViewBag.CurrentInfo = info;
                ViewBag.CurrentInfoJSON = Serializer.ToJSON(info);
            }

            ViewBag.Header = data;
           


        }
    }
}
