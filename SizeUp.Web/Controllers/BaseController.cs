using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Serialization;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;


namespace SizeUp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Core.DataLayer.Models.CurrentInfo CurrentInfo { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            SizeUp.Core.API.APISession.Create();
            var data = new Models.Header();

            CurrentInfo = new Core.DataLayer.Models.CurrentInfo();
            CurrentInfo.CurrentIndustry = WebContext.Current.CurrentIndustry;
            CurrentInfo.CurrentPlace = WebContext.Current.CurrentPlace;
            ViewBag.CurrentInfo = CurrentInfo;
            ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
            ViewBag.Header = data;


            SizeUp.Core.Analytics.PageViewToken t = new Core.Analytics.PageViewToken();
            t.IndustryId = WebContext.Current.CurrentIndustry != null ? WebContext.Current.CurrentIndustry.Id : (long?)null;
            var currentPlace = WebContext.Current.CurrentPlace;
            if (currentPlace.Id != null)
            {
                t.GeographicLocationId = currentPlace.Id;
            }
            else if (currentPlace.County != null && currentPlace.County.Id != null)
            {
                t.GeographicLocationId = currentPlace.County.Id;
            }
            else if (currentPlace.Metro != null && currentPlace.Metro.Id != null)
            {
                t.GeographicLocationId = currentPlace.Metro.Id;
            }
            else if (currentPlace.State != null && currentPlace.State.Id != null)
            {
                t.GeographicLocationId = currentPlace.State.Id;
            }
            ViewBag.TrackingToken = Server.UrlEncode(t.GetToken());
               
        }
    }      
}
