using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class BaseController : SizeUp.Web.Controllers.BaseController
    {
        //
        // GET: /Widget/Base/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var c = requestContext.HttpContext.Request.Cookies["theme"];
            var theme = "";
            if (c != null)
            {
                theme = c.Value;
            }
            ViewBag.Theme = theme.ToLower();

            //best places?
            ViewBag.WidgetToken = APIContext.Current.WidgetToken != null ? HttpUtility.UrlEncode(APIContext.Current.WidgetToken.GetToken()) : "";

            // if (Core.API.APIContext.Current.ApiToken != null && !Core.API.APIContext.Current.ApiToken.IsValid)
            // {
            //throw new HttpException(403, "Invalid API Key");
            // }
            base.Initialize(requestContext);
        }

    }
}
