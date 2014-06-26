using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Serialization;
using Api = SizeUp.Web.Areas.Api;

namespace SizeUp.Web.Controllers
{
    public class DemoController : BaseController
    {
        public string authToken = "ftyTAnI86i";
        List<string> countries = new List<string>() { "italy", "germany"};
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            var country = Request.QueryString["c"];
            var token = Request.QueryString["t"];

            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(country) || token != authToken || !countries.Contains(country) )
                return Redirect("/");

            return View();
        }

    }
}
