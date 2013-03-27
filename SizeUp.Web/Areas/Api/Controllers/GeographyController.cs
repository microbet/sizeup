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
    public class GeographyController : Controller
    {
        //
        // GET: /Api/Geography/

        public ActionResult Centroid(long id, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.Centroid(context, granularity).Where(i => i.Key == id).Select(i=>i.Value).FirstOrDefault();
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BoundingBox(long id, Granularity granularity)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Geography.BoundingBox(context, granularity).Where(i => i.Key == id).Select(i=>i.Value).FirstOrDefault();
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
