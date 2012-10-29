using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TopPlacesController : Controller
    {
        //
        // GET: /Api/TopPlaces/

        public ActionResult City()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Cities.Select(i=>
                new {
                    i.Id,
                    i.Name,
                    i.SEOKey,
                    LatLng = i.CityGeographies.Where(g=>g.GeographyClass.Name == "Calculation").Select(g=> new { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong}).FirstOrDefault(),
                    State = new {
                        i.State.Id,
                        i.State.Name,
                        i.State.SEOKey
                    }
                    
                }).Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult County()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Counties.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Metro()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Metroes.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult State()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.States.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
