using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using Microsoft.SqlServer.Types;
namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Wiget/Dashboard/

        public ActionResult Index()
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
