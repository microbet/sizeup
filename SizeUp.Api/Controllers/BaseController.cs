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

namespace SizeUp.Api.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsJsonp
        {
            get { return HttpContext.Request.QueryString[ConfigurationManager.AppSettings["API.CallbackName"]] != null; }
        }
        
        protected APIToken ApiToken
        {
            get
            {
                var tokenString = HttpContext.Request.QueryString[ConfigurationManager.AppSettings["API.TokenName"]];
                return APIToken.GetToken(tokenString);
            }
        }

        protected string Origin
        {
            get
            {
                return HttpContext.Request.QueryString[ConfigurationManager.AppSettings["API.OriginName"]];
            }
        }

        protected string SessionId
        {
            get
            {
                return HttpContext.Request.QueryString[ConfigurationManager.AppSettings["API.SessionName"]];
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //this tells IE NOT to cache requests becuase good ol microsoft knows best and goes against the grain
            //theres no WAY an ajax request could EVER return different data right?
            //suckit IE
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
            bool valid = false;
            if (IsJsonp)
            {
                Log();
            }
            valid = ValidateToken() && IsJsonp && ApiToken != null;
            if (!valid)
            {
                throw new HttpException(401, "Api token not valid");
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            JsonResult r = null;
            if (IsJsonp)
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
      
        protected bool ValidateToken()
        {
            var now = DateTime.UtcNow;
            var old = new DateTime(ApiToken.TimeStamp);

            var diff = now - old;
            var minutes = (int)diff.TotalMinutes;
            bool isValid = false;

            if (minutes < int.Parse(ConfigurationManager.AppSettings["Api.TokenExpiration"]))
            {
                if (!HttpContext.Request.IsLocal)
                {
                    using (var context = ContextFactory.SizeUpContext)
                    {
                        isValid = context.APIKeys
                             .Where(i => i.Id == ApiToken.APIKeyId)
                            //.Where(i => i.APIKeyDomains.Any(d => d.Domain == Origin))
                             .Count() > 0;
                    }
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        protected void Log()
        {
            Data.Analytics.APIRequest reg = new Data.Analytics.APIRequest();
            reg.OriginUrl = Origin;
            reg.Session = SessionId;
            reg.Url = HttpContext.Request.Url.OriginalString;
            reg.APIKeyId = ApiToken != null ? ApiToken.APIKeyId : (long?)null;
            Singleton<Tracker>.Instance.APIRequest(reg);
        }
    }
}
