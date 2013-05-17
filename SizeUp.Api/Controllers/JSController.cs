using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.API;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            //create an api database that has the apikey and domain authorizations as well as all the api logs



            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}", apikey.ToString(), Guid.NewGuid().ToString());
            bf.Serialize(s, data);
            SHA1CryptoServiceProvider a = new SHA1CryptoServiceProvider();
            var sessionid = Convert.ToBase64String(a.ComputeHash(s.ToArray()));           
            ViewBag.SessionId = sessionid;
            ViewBag.Token = APIToken.Create(apikey).GetToken();
            return View();
        }

        public ActionResult Data()
        {
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
