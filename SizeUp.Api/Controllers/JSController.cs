using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.API;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SizeUp.Data;

namespace SizeUp.Api.Controllers
{
    public class JSController : Controller
    {
        //
        // GET: /JS/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Response.ContentType = "text/javascript";
        }

        public ActionResult Index(Guid apikey)
        {
            APIToken token = null;
            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}", apikey.ToString(), Guid.NewGuid().ToString());
            bf.Serialize(s, data);
            SHA1CryptoServiceProvider a = new SHA1CryptoServiceProvider();
            var sessionid = Convert.ToBase64String(a.ComputeHash(s.ToArray()));           
            ViewBag.SessionId = sessionid;


            using (var context = ContextFactory.APIContext)
            {
                var k = context.APIKeys.Where(i => i.KeyValue == apikey).FirstOrDefault();
                if (k == null)
                {
                    throw new Exception("Invalid API Key");
                }
                token = new APIToken(k.Id);
            }
            ViewBag.Token = token.GetToken();
            return View();
        }

        public ActionResult Data()
        {
            if (APIContext.Current.ApiToken.IsValid && !APIContext.Current.ApiToken.IsExpired)
            {
                ViewBag.Permissions = new APIPermissions(APIContext.Current.ApiToken.APIKeyId);
            }
            else
            {
                throw new Exception("Invalid API Key");
            }
            return View();
        }

        public ActionResult Range()
        {
            return View();
        }

        public ActionResult Granularity()
        {
            return View();
        }

    }
}
