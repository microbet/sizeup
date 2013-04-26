using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.Web;

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
        

        public ActionResult Index()
        {
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

    }
}
