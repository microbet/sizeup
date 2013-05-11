using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class JSAPIController : Controller
    {
      

        public ActionResult Index()
        {
            
            return File("~/Scripts/jsapi/loader.js", "text/javascript");
        }


       

    }
}
