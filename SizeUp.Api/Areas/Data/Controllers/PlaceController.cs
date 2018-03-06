using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using System.Data.Objects;
using System.Data.Spatial;
using SizeUp.Core;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class PlaceController : BaseController
    {
        //
        // GET: /Api/Place/

        [APIAuthorize(Role = "Place")]
        public ActionResult Search(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Search(context, term).Take(maxResults).ToList();
                var query = ((ObjectQuery)(Core.DataLayer.Place.Search(context, term))).ToTraceString();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIAuthorize(Role = "Place")]
        public ActionResult SearchDetected(double lat, double lng)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = Core.DataLayer.Place.GetBoundingCity(context, new LatLng() { Lat = lat, Lng = lng });
                return Json(place, JsonRequestBehavior.AllowGet);
            }
        }

        [APIAuthorize(Role = "Place")]
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


        [APIAuthorize(Role = "Place")]
        public ActionResult Index(List<long> id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [APIAuthorize(Role = "Place")]
        public ActionResult GetBySeokey(string stateSeokey, string countySeokey, string placeSeokey)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Place.Get(context, stateSeokey, countySeokey, placeSeokey);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
