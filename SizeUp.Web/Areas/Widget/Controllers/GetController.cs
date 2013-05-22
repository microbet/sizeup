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
            Response.ContentType = "text/javascript";
        }

        public ActionResult Index(Guid key)
        {
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
            CreateToken(key);

            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}", key.ToString(), Guid.NewGuid().ToString());
            bf.Serialize(s, data);
            SHA1CryptoServiceProvider a = new SHA1CryptoServiceProvider();
            var sessionid = Convert.ToBase64String(a.ComputeHash(s.ToArray()));
            ViewBag.SessionId = sessionid;

            return View();
        }


        public ActionResult BestPlaces()
        {
            return View();
        }

        protected void CreateToken(Guid key)
        {
            using (var context = ContextFactory.APIContext)
            {
                var api = context.APIKeys.Where(i => i.KeyValue == key).FirstOrDefault();
                if (api != null)
                {
                    ViewBag.APIName = api.Name;
                    var token = new APIToken(api.Id);
                    token.PersistAsCookie();
                    ViewBag.Token = token.GetToken();
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
