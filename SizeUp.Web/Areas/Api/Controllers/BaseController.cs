using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;
using System.Configuration;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class BaseController : Controller
    {
       
        /*
        protected JsonResult BuildJson(object data, JsonRequestBehavior? behavior)
        {
            JsonResult result = null;
            if (IsJsonp)
            {
                result = new JsonpResult();
                result.Data = data;
                if (behavior.HasValue)
                {
                    result.JsonRequestBehavior = behavior.Value;
                }
            }
            else
            {
                result = new JsonResult();
                result.Data = data;
                if (behavior.HasValue)
                {
                    result.JsonRequestBehavior = behavior.Value;
                }
            }
            return result;
        }
        */

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //this tells IE NOT to cache requests becuase good ol microsoft knows best and goes against the grain
            //theres no WAY an ajax request could EVER return different data right?
            //suckit IE
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
        }

        /*
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
           //HERE we need to test the incomming api key and 
            //verify its good
            //log the request
            //and then process the request
            return BuildJson(data, behavior);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return BuildJson(data, null);
        }
         */
    }
}
