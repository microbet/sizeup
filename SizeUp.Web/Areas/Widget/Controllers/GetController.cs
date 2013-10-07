using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;
using SizeUp.Data;
using SizeUp.Core.API;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class GetController : Controller
    {
        //
        // GET: /Widget/Get/
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            requestContext.HttpContext.Response.AddHeader("Expires", "-1");
            Response.ContentType = "text/javascript";
            APISession.Create();  
        }

        public ActionResult Index(Guid key)
        {
            CreateToken(key);
            ViewBag.SessionId = APISession.Current.SessionId;
            ViewBag.InstanceId = RandomString.Get(25);

            //sets a test cookie to see if cookies are enabled.
            //this cookie is read on the load page and is used to determine if "browser authorization" is required for safari
            Response.Cookies.Add(SizeUp.Core.Web.CookieFactory.Create("enabled", "true"));

            return View();
        }


        public ActionResult BestPlaces()
        {
            HttpCookie c = SizeUp.Core.Web.CookieFactory.Create("theme");
            c.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(c);
            return View();
        }

        public ActionResult BestIndustries(string state, string county, string city)
        {
            HttpCookie c = new HttpCookie("theme");
            c.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(c);
            ViewBag.State = state;
            ViewBag.County = county;
            ViewBag.City = city;

            return View();
        }

        protected void CreateToken(Guid key)
        {
            using (var context = ContextFactory.APIContext)
            {
                if (!context.APIKeyRoleMappings.Any(i => i.APIKey.KeyValue == key && i.Role.Name.ToLower() == "widget"))
                {
                    throw new HttpException(403, "Not authorized to use the widget");
                }


                var api = context.APIKeys.Where(i => i.KeyValue == key).FirstOrDefault();
                if (api != null)
                {
                    ViewBag.APIName = api.Name;
                    var token = APIToken.Create(api.Id);
                    ViewBag.Token = token.GetToken();
                }
                else
                {
                    throw new HttpException(403, "Invalid API Key");
                }

               

            }
        }

    }
}
