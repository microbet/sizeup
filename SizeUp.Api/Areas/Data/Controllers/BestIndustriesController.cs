using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class BestIndustriesController : BaseController
    {
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Index(long geographicLocationId, string attribute = "TotalRevenue", int count = 5)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.BestIndustries.Get(context, geographicLocationId, attribute)
                    .Take(5)
                    .ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
