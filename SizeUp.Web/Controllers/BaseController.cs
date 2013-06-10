using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Serialization;
using SizeUp.Core.DataLayer;

namespace SizeUp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Core.DataLayer.Models.CurrentInfo CurrentInfo { get; set; }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var data = new Models.Header();


            using (var context = ContextFactory.SizeUpContext)
            {
                //if a bug comes up again with this we should concider moving it to WebContext and have that pull the data from the DB
                long placeId = 0;
                long industryId = 0;
                var city = (string)requestContext.RouteData.Values["city"];
                var county = (string)requestContext.RouteData.Values["county"];
                var state = (string)requestContext.RouteData.Values["state"];
                var industry = (string)requestContext.RouteData.Values["industry"];
                string feature = HttpContext.Request.QueryString["Feature"];

                CurrentInfo = new Core.DataLayer.Models.CurrentInfo();
                if (long.TryParse(requestContext.HttpContext.Request["placeId"], out placeId))
                {
                    CurrentInfo.CurrentPlace = Core.DataLayer.Place.Get(context, placeId);
                }
                else
                {
                    CurrentInfo.CurrentPlace = Core.DataLayer.Place.Get(context, state, county, city);
                }

                if (long.TryParse(requestContext.HttpContext.Request["industryId"], out industryId))
                {
                    CurrentInfo.CurrentIndustry = Core.DataLayer.Industry.Get(context, industryId);
                }
                else
                {
                    CurrentInfo.CurrentIndustry = Core.DataLayer.Industry.Get(context, industry);
                }

                if (CurrentInfo.CurrentPlace.Id != null)
                {
                    WebContext.Current.CurrentPlaceId = CurrentInfo.CurrentPlace.Id;
                }
                if (CurrentInfo.CurrentIndustry.Id != null)
                {
                    WebContext.Current.CurrentIndustryId = CurrentInfo.CurrentIndustry.Id;
                }


                if (feature != null && feature.ToLower() == "dashboard")
                {
                    WebContext.Current.StartFeature = Feature.Dashboard;
                }
                else if (feature != null && feature.ToLower() == "competition")
                {
                    WebContext.Current.StartFeature = Feature.Competition;
                }
                else if (feature != null && feature.ToLower() == "community")
                {
                    WebContext.Current.StartFeature = Feature.Community;
                }
                else if (feature != null && feature.ToLower() == "advertsing")
                {
                    WebContext.Current.StartFeature = Feature.Advertising;
                }
                else if (feature != null && feature.ToLower() == "select")
                {
                    WebContext.Current.StartFeature = Feature.Select;
                }
              
                ViewBag.CurrentInfo = CurrentInfo;
                ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
                ViewBag.Header = data;
            }      
        }
    }      
}
