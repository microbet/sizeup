using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Error/

        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            return View();
        }

        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            ViewBag.Header = new Models.Header()
            {
                HideMenu = true
            };
            return View();
        }

    }
}
