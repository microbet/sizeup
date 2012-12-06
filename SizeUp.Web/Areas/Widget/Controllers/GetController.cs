using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;

namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class GetController : Controller
    {
        //
        // GET: /Widget/Get/

        public ActionResult Index()
        {
            Guid g;
            if (!Guid.TryParse(Request["key"], out g))
            {
                g = Guid.Empty;
            }
            string theme = Request.QueryString["theme"];
            if (!string.IsNullOrWhiteSpace(theme))
            {
                HttpCookie c = new HttpCookie("theme", theme);
                Response.Cookies.Add(c);
            }
            else
            {

                HttpCookie c = new HttpCookie("theme");
                c.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c);

            }
            WidgetToken.Create(g);
            return File("~/Scripts/widget/embed.js", "text/javascript");
        }


        public ActionResult TopPlaces()
        {
            return File("~/Scripts/widget/topPlaces.js", "text/javascript");
        }

    }
}
