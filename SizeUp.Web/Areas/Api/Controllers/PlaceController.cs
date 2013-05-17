using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using Microsoft.SqlServer.Types;
using System.Data.Objects;
using System.Data.Spatial;
using SizeUp.Core;
using SizeUp.Core.API;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class PlaceController : BaseController
    {
        //
        // GET: /Api/Place/
        [HttpGet]
        public ActionResult Current()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                 var data = Core.DataLayer.Place.Get(context, Core.Web.WebContext.Current.CurrentPlaceId);
                 return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Current(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = context.CityCountyMappings.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentPlaceId = id;
                }
                return Json(c != null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detected()
        {
            var ip = Request.Headers["X-Forwarded-For"];
            var id = GeoCoder.GetPlaceIdByIPAddress(ip);
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
