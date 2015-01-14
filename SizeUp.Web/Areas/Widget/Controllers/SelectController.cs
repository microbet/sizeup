using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Controllers;
using SizeUp.Web.Models;
using SizeUp.Core.Web;
using SizeUp.Core.API;
using SizeUp.Data;
namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class SelectController : BaseController
    {
        //
        // GET: /Wiget/Select/

        public ActionResult Index()
        {
            ViewBag.Header = new Models.Header()
            {
                HideNavigation = true
            };

            if (APIContext.Current.WidgetToken != null)
            {
                using (var context = ContextFactory.APIContext)
                {
                    var currentKey = APIContext.Current.WidgetToken.APIKeyId;
                    var keyValue = context.APIKeys.Where(i => i.Id == currentKey).Select(i => i.KeyValue).FirstOrDefault();
                    ViewBag.APIKeyValue = keyValue;
                }
            }

            ViewBag.CustomTools = APIContext.Current.WidgetPermissions.CustomTools;
            ViewBag.StartFeature = WebContext.Current.StartFeature;
            return View();
        }

    }
}
