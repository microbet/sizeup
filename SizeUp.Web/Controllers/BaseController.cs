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

                var city = (string)requestContext.RouteData.Values["city"];
                var county = (string)requestContext.RouteData.Values["county"];
                var state = (string)requestContext.RouteData.Values["state"];
                var industry = (string)requestContext.RouteData.Values["industry"];

                CurrentInfo = new Core.DataLayer.Models.CurrentInfo()
                {
                    CurrentIndustry = Core.DataLayer.Industry.Get(context, industry),
                    CurrentPlace = Core.DataLayer.Place.Get(context, state,county, city)
                };

                if (CurrentInfo.CurrentPlace.Id != null)
                {
                    WebContext.Current.CurrentPlaceId = CurrentInfo.CurrentPlace.Id;
                }
                if (CurrentInfo.CurrentIndustry.Id != null)
                {
                    WebContext.Current.CurrentIndustryId = CurrentInfo.CurrentIndustry.Id;
                }

                ViewBag.CurrentInfo = CurrentInfo;
                ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
                ViewBag.Header = data;
            }
        }
    }      
}
