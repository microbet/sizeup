using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SizeUp.Web.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Business/

        public ActionResult StateList()
        {
            return View();
        }

        public ActionResult CityList(string state)
        {
            return View();
        }

        public ActionResult IndustryList(string state, string city)
        {
            return View();
        }

    }
}
