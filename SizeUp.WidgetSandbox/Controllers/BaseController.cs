﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SizeUp.WidgetSandbox.Controllers
{
    public class BaseController : Controller
    {
       
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ViewData["url"] = Request.IsLocal ? "localhost:55040" : "beta.sizeup.com";

        }
    }
}
