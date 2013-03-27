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

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class PlaceController : BaseController
    {
        //
        // GET: /Api/Place/

        public ActionResult Search(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Search(context, term).Take(maxResults).ToList();
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Current()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                 var data = Core.DataLayer.Place.Get(context, Core.Web.WebContext.Current.CurrentPlaceId);
                 return this.Jsonp(data, JsonRequestBehavior.AllowGet);
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
                return this.Jsonp(c != null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detected()
        {
            var id = GeoCoder.GetPlaceIdByIPAddress();
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, id);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Index(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, id);
                return this.Jsonp(data, JsonRequestBehavior.AllowGet);
            }
        }




    }
}
