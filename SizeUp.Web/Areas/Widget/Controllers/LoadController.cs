using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.Web;
using SizeUp.Core.API;
using SizeUp.Data.Analytics;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class LoadController : BaseController
    {
        //
        // GET: /Widget/Load/

        protected long? PlaceId
        {
            get
            {
                long id = 0;
                long? output = null;
                if (long.TryParse(HttpContext.Request.QueryString["placeId"], out id))
                {
                    output = id;
                }
                return output;
            }
        }

        protected long? IndustryId
        {
            get
            {
                long id = 0;
                long? output = null;
                if (long.TryParse(HttpContext.Request.QueryString["industryid"], out id))
                {
                    output = id;
                }
                return output;
            }
        }

        protected Feature? StartFeature
        {
            get
            {
                Feature? output = null;
                string param = HttpContext.Request.QueryString["Feature"];

                if (param != null && param.ToLower() == "dashboard")
                {
                    output = Feature.Dashboard;
                }
                else if (param != null && param.ToLower() == "competition")
                {
                    output = Feature.Competition;
                }
                else if (param != null && param.ToLower() == "community")
                {
                    output = Feature.Community;
                }
                else if (param != null && param.ToLower() == "advertsing")
                {
                    output = Feature.Advertising;
                }
                else if (param != null && param.ToLower() == "select")
                {
                    output = Feature.Select;
                }
                return output;
            }
        }
        

        [APIAuthorize(Role = "Widget")]
        public ActionResult Index()
        {
            bool valid = APIContext.Current.ApiToken != null && APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired;
            Log();
            if (!valid)
            {
                throw new HttpException(401, "Api token not valid");
            }



            using (var context = ContextFactory.SizeUpContext)
            {
                string urlBase = "/{0}/{1}/{2}/{3}";
                string url = "/widget/select";
                if (PlaceId != null)
                {
                    WebContext.Current.CurrentPlaceId = PlaceId;
                }
                if (IndustryId != null)
                {
                    WebContext.Current.CurrentIndustryId = IndustryId;
                }
                WebContext.Current.StartFeature = StartFeature;

                if (PlaceId != null && IndustryId != null)
                {
                    var place = Core.DataLayer.Place.Get(context, PlaceId);
                    var industry = Core.DataLayer.Industry.Get(context, IndustryId);
                    urlBase = string.Format(urlBase, place.State.SEOKey, place.County.SEOKey, place.City.SEOKey, industry.SEOKey);

                    if (StartFeature == Feature.Advertising)
                    {
                        url = string.Format("{0}{1}", "/widget/advertising", urlBase);
                    }
                    else if (StartFeature == Feature.Competition)
                    {
                        url = string.Format("{0}{1}", "/widget/competition", urlBase);
                    }
                    else if (StartFeature == Feature.Dashboard)
                    {
                        url = string.Format("{0}{1}", "/widget/dashboard", urlBase);
                    }
                    else if (StartFeature == Feature.Community)
                    {
                        url = string.Format("{0}{1}", "/widget/community", urlBase);
                    }
                }
                return Redirect(url);
            }
        }

        protected void Log()
        {
            Data.Analytics.APIRequest reg = new Data.Analytics.APIRequest();
            reg.OriginUrl = APIContext.Current.Origin;
            reg.Session = APIContext.Current.Session;
            reg.Url = HttpContext.Request.Url.OriginalString;
            reg.APIKeyId = APIContext.Current.ApiToken != null ? APIContext.Current.ApiToken.APIKeyId : (long?)null;
            Singleton<Tracker>.Instance.APIRequest(reg);
        }

    }
}
