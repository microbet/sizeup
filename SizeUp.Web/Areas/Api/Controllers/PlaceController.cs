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
                var data = Core.Web.WebContext.Current.CurrentPlace.Id != null ? Core.Web.WebContext.Current.CurrentPlace : null;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Current(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                /*
                var c = context.CityCountyMappings.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentPlaceId = id;
                }*/
                //depricated
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detected()
        {
            HttpContext.Server.ScriptTimeout = 3;
            var id = GeoCoder.GetPlaceIdByIPAddress(WebContext.Current.ClientIP);
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
