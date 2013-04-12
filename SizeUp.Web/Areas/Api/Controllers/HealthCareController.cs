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

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class HealthCareController : BaseController
    {
        //
        // GET: /Api/HealthCare/
        [LogAPIRequest]
        [ValidateAPIRequest]
        [AllowAPIRequest]
        public ActionResult Chart(long industryId, long placeId, long? employees, Granularity granularity = Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Healthcare.Chart(context, industryId, placeId, employees, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [LogAPIRequest]
        [ValidateAPIRequest]
        [AllowAPIRequest]
        public ActionResult Percentage(int industryId, long placeId, long value, Granularity granularity = Granularity.State)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.Healthcare.Percentage(context, industryId, placeId, value, granularity);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
