using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Core;
using SizeUp.Data;
using SizeUp.Core.Web;
namespace SizeUp.Web.Areas.Widget.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Widget/Community/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.BusinessCount = Core.DataLayer.Business.ListIn(context, WebContext.Current.CurrentIndustry.Id, WebContext.Current.CurrentPlace.Id.Value).Count();
                return View();
            }
        }

    }
}
