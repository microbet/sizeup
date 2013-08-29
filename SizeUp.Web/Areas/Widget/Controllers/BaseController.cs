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



        protected APIToken APIToken { get { return Core.API.APIToken.GetFromCookie(); } }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var c = requestContext.HttpContext.Request.Cookies["theme"];
            var theme = "";
            if (c != null)
            {
                theme = c.Value;
            }
            ViewBag.Theme = theme.ToLower();


            using (var context = ContextFactory.APIContext)
            {
                var currentKey = APIToken.APIKeyId;
                var api = context.APIKeys.Where(i => i.Id == currentKey).Select(i => i.Name).FirstOrDefault();
                ViewBag.APIName = api;
            }

            if (APIToken != null && !APIToken.IsValid)
            {
                //throw new HttpException(403, "Invalid API Key");
            }
            base.Initialize(requestContext);
        }

    }
}
