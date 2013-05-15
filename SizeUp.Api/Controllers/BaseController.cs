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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //this tells IE NOT to cache requests becuase good ol microsoft knows best and goes against the grain
            //theres no WAY an ajax request could EVER return different data right?
            //suckit IE
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
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
    }
}
