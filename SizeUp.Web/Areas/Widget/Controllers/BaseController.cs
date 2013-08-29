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
            ViewBag.WidgetToken = "";

            if (APIContext.Current.WidgetToken != null)
            {
                using (var context = ContextFactory.APIContext)
                {
                    var currentKey = APIContext.Current.WidgetToken.APIKeyId;
                    var api = context.APIKeys.Where(i => i.Id == currentKey).Select(i => i.Name).FirstOrDefault();
                    ViewBag.APIName = api;
                }
                ViewBag.WidgetToken = HttpUtility.UrlEncode(APIContext.Current.WidgetToken.GetToken());
            }
            // if (Core.API.APIContext.Current.ApiToken != null && !Core.API.APIContext.Current.ApiToken.IsValid)
            // {
            //throw new HttpException(403, "Invalid API Key");
            // }
            base.Initialize(requestContext);
        }

    }
}
