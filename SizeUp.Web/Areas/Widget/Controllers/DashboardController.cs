using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using Microsoft.SqlServer.Types;
using SizeUp.Core.API;
using SizeUp.Data;
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
                if (APIContext.Current.WidgetToken != null)
                {
                    using (var context2 = ContextFactory.APIContext)
                    {
                        var currentKey = APIContext.Current.WidgetToken.APIKeyId;
                        var keyValue = context2.APIKeys.Where(i => i.Id == currentKey).Select(i => i.KeyValue).FirstOrDefault();
                        ViewBag.APIKeyValue = keyValue;
                    }
                }

                ViewBag.CustomTools = APIContext.Current.WidgetPermissions.CustomTools;
                ViewBag.Strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);
                ViewBag.Header.ActiveTab = NavItems.Dashboard;
                return View();
            }
        }
    }
}
