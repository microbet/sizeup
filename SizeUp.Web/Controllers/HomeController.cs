﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core.Web;
using SizeUp.Core.Serialization;
using Api = SizeUp.Web2.Areas.Api.Controllers;

namespace SizeUp.Web2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            
            var dc = WebContext.Current.DetectedCity;
            var cc = WebContext.Current.CurrentCity;
            var ci = WebContext.Current.CurrentIndustry;


            ViewBag.DetectedCityId = dc != null ? dc.Id.ToString() : "null";
            ViewBag.CurrentCityId = cc != null ? cc.Id.ToString() : "null";
            ViewBag.CurrentIndustryId = ci != null ? ci.Id.ToString() : "null";


            return View();
        }

    }
}
