﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;

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


            using (var context = ContextFactory.APIContext)
            {
                var currentKey = WidgetToken.APIKey;
                var api = context.APIKeys.Where(i => i.KeyValue == currentKey).Select(i=>i.Name).FirstOrDefault();
                ViewBag.APIName = api;
            }
            base.Initialize(requestContext);
        }

    }
}
