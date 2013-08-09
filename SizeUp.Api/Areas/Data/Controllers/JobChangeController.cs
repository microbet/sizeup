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
    public class JobChangeController : BaseController
    {
        //
        // GET: /Api/JobChange/

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(int industryId, int geographicLocationId )
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.JobChange.Chart(context, industryId, geographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.JobChange.Percentile(context, industryId, geographicLocationId, boundingGeographicLocationId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
