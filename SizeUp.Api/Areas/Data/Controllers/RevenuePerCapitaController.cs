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
    public class RevenuePerCapitaController : BaseController
    {
        //
        // GET: /Api/RevenuePerCapita/

        [APIRequest]
        public ActionResult Chart(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.RevenuePerCapita.Chart(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        [APIRequest]
        public ActionResult Percentile(long industryId, long placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.RevenuePerCapita.Percentile(context, industryId, placeId, granularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        [APIRequest]
        public ActionResult Bands(long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity = Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.RevenuePerCapita.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
