﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using Microsoft.SqlServer.Types;

namespace SizeUp.Web.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/

        public ActionResult Index(string state, string city, string industry)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);               
                ViewBag.Header.ActiveTab = NavItems.Dashboard;
                return View();
            }
        }

    }
}
