using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Api/User/

        public ActionResult Authenticated()
        {
            var output = User.Identity.IsAuthenticated;
            return Json(output, JsonRequestBehavior.AllowGet);
        }

    }
}
