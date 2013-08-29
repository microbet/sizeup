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
            APISession.Create();  
        }

        public ActionResult Index(Guid key, long? industryId = null, long? placeId = null, string theme = null, string feature = "")
        {
            Feature? startFeature = null;
            if (feature.ToLower() == "dashboard")
            {
                startFeature = Feature.Dashboard;
            }
            else if (feature.ToLower() == "competition")
            {
                startFeature = Feature.Competition;
            }
            else if (feature.ToLower() == "community")
            {
                startFeature = Feature.Community;
            }
            else if (feature.ToLower() == "advertsing")
            {
                startFeature = Feature.Advertising;
            }
            else if (feature.ToLower() == "select")
            {
                startFeature = Feature.Select;
            }
            WebContext.Current.StartFeature = startFeature;




            if (placeId != null)
            {
                var place = new Core.DataLayer.Models.Place() { Id = placeId };
                WebContext.Current.CurrentPlace = place;
            }
            if (industryId != null)
            {
                var industry = new Core.DataLayer.Models.Industry() { Id = (long)industryId };
                WebContext.Current.CurrentIndustry = industry;
            }

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
            ViewBag.SessionId = APISession.Current.SessionId;
            ViewBag.InstanceId = RandomString.Get(25);

            return View();
        }


        public ActionResult BestPlaces()
        {
            HttpCookie c = new HttpCookie("theme");
            c.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(c);
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
