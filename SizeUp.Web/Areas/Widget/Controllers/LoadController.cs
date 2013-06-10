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

                if (WebContext.Current.CurrentPlaceId != null && WebContext.Current.CurrentIndustryId != null)
                {
                    var place = Core.DataLayer.Place.Get(context, WebContext.Current.CurrentPlaceId);
                    var industry = Core.DataLayer.Industry.Get(context, WebContext.Current.CurrentIndustryId);
                    urlBase = string.Format(urlBase, place.State.SEOKey, place.County.SEOKey, place.City.SEOKey, industry.SEOKey);

                    if (WebContext.Current.StartFeature == Feature.Advertising)
                    {
                        url = string.Format("{0}{1}", "/widget/advertising", urlBase);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Competition)
                    {
                        url = string.Format("{0}{1}", "/widget/competition", urlBase);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Dashboard)
                    {
                        url = string.Format("{0}{1}", "/widget/dashboard", urlBase);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Community)
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
