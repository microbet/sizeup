using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AverageEmployeesController : BaseController
    {
        //
        // GET: /Api/Employee/

        public ActionResult Chart(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageEmployees.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentile(long industryId, long placeId, long value, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageEmployees.Percentile(context, industryId, placeId, value, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bands(long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity = Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.AverageEmployees.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
