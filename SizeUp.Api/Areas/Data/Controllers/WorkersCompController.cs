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
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class WorkersCompController : BaseController
    {
        //
        // GET: /Api/WorkersComp/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long placeId, Granularity granularity = Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.WorkersComp.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentage(int industryId, long placeId, double value, Granularity granularity = Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.WorkersComp.Percentage(context, industryId, placeId, value, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
