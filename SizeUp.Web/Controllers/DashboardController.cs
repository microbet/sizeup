using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using Microsoft.SqlServer.Types;

namespace SizeUp.Web.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/

        public ActionResult Index(string state, string city, string industry, string businessStatus)
        {
            if (CurrentInfo.CurrentPlace.Id == null || CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);               
                ViewBag.Header.ActiveTab = NavItems.Dashboard;
                return View();
            }
        }

    }
}
