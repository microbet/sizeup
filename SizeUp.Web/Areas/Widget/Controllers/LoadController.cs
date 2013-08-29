﻿using System;
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
using SizeUp.Core.Analytics;
using System.Web.Security;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class LoadController : BaseController
    {
        //
        // GET: /Widget/Load/

        [APIAuthorize(Role = "Widget")]
        public ActionResult Index()
        {
            //in this context the APIToken is the widget key
            bool valid = APIContext.Current.ApiToken != null && APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired;
            Log();
            if (!valid)
            {
                throw new HttpException(401, "Api token not valid");
            }



            using (var context = ContextFactory.SizeUpContext)
            {
                string urlToken = HttpUtility.UrlEncode(APIContext.Current.ApiToken.GetToken());
                string urlBase = "/{0}/{1}/{2}/{3}";
                string url = string.Format("/{0}?wt={1}","widget/select", urlToken);

                if (WebContext.Current.CurrentPlace.Id != null && WebContext.Current.CurrentIndustry != null)
                {
                    var place = WebContext.Current.CurrentPlace;
                    var industry = WebContext.Current.CurrentIndustry;
                    urlBase = string.Format(urlBase, place.State.SEOKey, place.County.SEOKey, place.City.SEOKey, industry.SEOKey);

                    if (WebContext.Current.StartFeature == Feature.Advertising)
                    {
                        url = string.Format("/{0}{1}?wt={2}", "widget/advertising", urlBase, urlToken);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Competition)
                    {
                        url = string.Format("/{0}{1}?wt={2}", "widget/competition", urlBase, urlToken);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Dashboard)
                    {
                        url = string.Format("/{0}{1}?wt={2}", "widget/dashboard", urlBase, urlToken);
                    }
                    else if (WebContext.Current.StartFeature == Feature.Community)
                    {
                        url = string.Format("/{0}{1}?wt={2}", "widget/community", urlBase, urlToken);
                    }
                }
                
                return Redirect(url);
            }
        }

        protected void Log()
        {
            Guid? userid = null;
            if (User.Identity.IsAuthenticated)
            {
                userid = (Guid)Membership.GetUser().ProviderUserKey;
            }

            Data.Analytics.APIRequest reg = new Data.Analytics.APIRequest();
            reg.OriginUrl = APIContext.Current.Origin;
            reg.Url = HttpContext.Request.Url.OriginalString;
            reg.OriginIP = WebContext.Current.ClientIP;
            reg.UserId = userid;
            Singleton<Tracker>.Instance.APIRequest(reg);
        }

    }
}
