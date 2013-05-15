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
    public class JSController : BaseController
    {
        //
        // GET: /JS/

        public ActionResult Index(Guid apikey)
        {
            //this should serve the loader and that will load all things needed for the api
            //move all data feeds to this yet continue to keep stuff like user and profile things in sizeup

            //create an api database that has the apikey and domain authorizations as well as all the api logs



            MemoryStream s = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            string data = string.Format("{0}|{1}", apikey.ToString(), Guid.NewGuid().ToString());
            bf.Serialize(s, data);
            SHA1CryptoServiceProvider a = new SHA1CryptoServiceProvider();
            var sessionid = Convert.ToBase64String(a.ComputeHash(s.ToArray()));
            Response.ContentType = "text/javascript";
            ViewBag.SessionId = sessionid;
            return View();
        }

    }
}
