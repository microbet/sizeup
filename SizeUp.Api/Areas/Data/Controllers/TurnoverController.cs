using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Core;
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class TurnoverController : BaseController
    {
        //
        // GET: /Api/Turnover/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Turnover.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Turnover.Percentile(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
