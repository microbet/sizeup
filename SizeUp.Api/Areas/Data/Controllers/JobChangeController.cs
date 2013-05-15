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
    public class JobChangeController : BaseController
    {
        //
        // GET: /Api/JobChange/

        [APIRequest]
        public ActionResult Chart(int industryId, int placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.JobChange.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIRequest]
        public ActionResult Percentile(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.JobChange.Percentile(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
