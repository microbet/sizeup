using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;
using SizeUp.Data;
using SizeUp.Core.API;

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
            CreateToken(g);
            return File("~/Scripts/widget/embed.js", "text/javascript");
        }


        public ActionResult BestPlaces()
        {
            return File("~/Scripts/widget/bestPlaces.js", "text/javascript");
        }

        protected void CreateToken(Guid key)
        {
            using (var context = ContextFactory.APIContext)
            {
                var api = context.APIKeys.Where(i => i.KeyValue == key).FirstOrDefault();
                //here we need to implement additional cehcking to make sure the domain this comes form is correct.
                if (api != null)
                {
                    ViewBag.APIName = api.Name;
                    var token = new APIToken(api.Id);
                    token.PersistAsCookie();
                }
                else
                {
                    throw new HttpException(403, "Invalid API Key");
                }

                if (!context.APIKeyRoleMappings.Any(i => i.APIKey.KeyValue == key && i.Role.Name.ToLower() == "widget"))
                {
                    throw new HttpException(403, "Not authorized to use the widget");
                }

            }
        }

    }
}
