using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class AuthorizeController : Controller
    {
        //
        // GET: /Widget/Authorize/

        public ActionResult Index()
        {
            //sets the cookie the widget/load action uses to determine if cookies are enabled.
            //this just sets a cookie, then reloads the browser this should bypass the third party cookie restrictions in safari
            Response.Cookies.Add(SizeUp.Core.Web.CookieFactory.Create("enabled", "true"));
            return View();
        }

    }
}
