using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SizeUp.Core;
using SizeUp.Core.API;
using SizeUp.Core.Web;
using SizeUp.Data;
using SizeUp.Data.Analytics;
using SizeUp.Core.Analytics;
using System.Web.Security;

namespace SizeUp.Api.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            APISession.Create();  
            //this tells IE NOT to cache requests becuase good ol microsoft knows best and goes against the grain
            //theres no WAY an ajax request could EVER return different data right?
            //suckit IE
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
            bool valid = false;
            Log();
            valid = APIContext.Current.ApiToken != null && APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired;
            if (!valid)
            {
                throw new HttpException(403, "Api token not valid");
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            JsonResult r = null;
            if (APIContext.Current.IsJsonp)
            {
                r = new JsonpResult();
                r.Data = data;
                r.JsonRequestBehavior = behavior;
                r.ContentEncoding = contentEncoding;
                r.ContentType = contentType;
            }
            else
            {
                r = new JsonResult();
                r.Data = data;
                r.JsonRequestBehavior = behavior;
                r.ContentEncoding = contentEncoding;
                r.ContentType = contentType;
            }
            return r;
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
