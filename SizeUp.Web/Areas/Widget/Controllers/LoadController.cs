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
using SizeUp.Core.Analytics;
using System.Web.Security;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class LoadController : BaseController
    {
        //
        // GET: /Widget/Load/

        [APIAuthorize(Role = "Widget")]
        public ActionResult Index(long? industryId = null, long? placeId = null, string theme = null, string feature = "")
        {
            //in this context the APIToken is the widget key
            bool valid = APIContext.Current.ApiToken != null && APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired;
            Log();
            if (!valid)
            {
                throw new HttpException(401, "Api token not valid");
            }


            if (Request.Cookies["enabled"] == null)
            {
                return View("~/areas/widget/views/Authorize/Authorize.cshtml");
            }
            HttpCookie cc = CookieFactory.Create("enabled");
            cc.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cc);


            Feature? startFeature = null;
            if (feature.ToLower() == "dashboard")
            {
                startFeature = Feature.Dashboard;
            }
            else if (feature.ToLower() == "competition")
            {
                startFeature = Feature.Competition;
            }
            else if (feature.ToLower() == "community")
            {
                startFeature = Feature.Community;
            }
            else if (feature.ToLower() == "advertsing")
            {
                startFeature = Feature.Advertising;
            }
            else if (feature.ToLower() == "featureselect")
            {
                startFeature = Feature.FeatureSelect;
            }
            else if (feature.ToLower() == "select")
            {
                startFeature = Feature.Select;
            }
            WebContext.Current.StartFeature = startFeature;




            if (placeId != null)
            {
                var place = new Core.DataLayer.Models.Place() { Id = placeId };
                WebContext.Current.CurrentPlace = place;
            }
            if (industryId != null)
            {
                var industry = new Core.DataLayer.Models.Industry() { Id = (long)industryId };
                WebContext.Current.CurrentIndustry = industry;
            }

            if (!string.IsNullOrWhiteSpace(theme))
            {
                HttpCookie c = SizeUp.Core.Web.CookieFactory.Create("theme", theme);
                Response.Cookies.Add(c);
            }
            else
            {
                HttpCookie c = SizeUp.Core.Web.CookieFactory.Create("theme");
                c.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c);
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
                    else if (WebContext.Current.StartFeature == Feature.FeatureSelect)
                    {
                        url = string.Format("/{0}?wt={1}#featureSelect=true","widget/select", urlToken);
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
