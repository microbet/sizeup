using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Api.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Error403()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }

    }
}
