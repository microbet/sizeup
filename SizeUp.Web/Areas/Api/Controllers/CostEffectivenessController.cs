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
    public class CostEffectivenessController : BaseController
    {
        //
        // GET: /Api/AverageRevenue/
        public ActionResult Chart(int industryId, int placeId, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.CostEffectiveness.Chart(context, industryId, placeId, granularity);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Percentage(int industryId, int placeId, int revenue, int employees, int salary, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var ce = revenue / (double)(employees * salary);
                var obj = Core.DataLayer.CostEffectiveness.Percentage(context, industryId, placeId, ce, granularity);
                return this.Jsonp(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bands(long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity = Granularity.Nation)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.CostEffectiveness.Bands(context, industryId, placeId, bands, granularity, boundingGranularity);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
