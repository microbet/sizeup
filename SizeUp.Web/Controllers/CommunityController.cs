using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Core.DataAccess;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;

namespace SizeUp.Web.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Community/

        public ActionResult CommunityWithIndustry()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var location = Locations.Get(context, WebContext.Current.CurrentPlaceId.Value).FirstOrDefault();
                ViewBag.BusinessCount = BusinessData.GetByCity(context, WebContext.Current.CurrentIndustryId.Value, location.City.Id).Count();
                return View();
            }
        }


        public ActionResult Community()
        {

            return View();
        }


        public ActionResult RedirectWithIndustry(string oldSEO, string industry)
        {
            return View();
        }

        public ActionResult Redirect(string oldSEO)
        {
            return View();
        }


    }
}
