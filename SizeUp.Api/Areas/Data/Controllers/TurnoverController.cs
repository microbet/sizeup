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
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class TurnoverController : BaseController
    {
        //
        // GET: /Api/Turnover/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long geographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Turnover.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Turnover.Percentile(context, industryId, geographicLocationId, boundingGeographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
