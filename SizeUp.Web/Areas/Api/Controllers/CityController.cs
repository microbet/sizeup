using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;

namespace SizeUp.Web2.Areas.Api.Controllers
{
    public class CityController : Controller
    {
        //
        // GET: /Api/City/

        public JsonResult GetCity(int? id)
        {
            var item = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id);
            var data = item.Select(i => new
            {
                i.Id,
                i.Name
            }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentCity()
        {
            var item = SizeUp.Core.Web.WebContext.Current.CurrentCity;
            object data = null;
            if (item != null)
            {
                data = new
               {
                   item.Id,
                   item.Name
               };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetectedCity()
        {
            var item = SizeUp.Core.Web.WebContext.Current.DetectedCity;
            object data = null;
            if (item != null)
            {
                data = new
                {
                    item.Id,
                    item.Name
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCities(string term, int maxResults = 35)
        {
            var keywords = DataContexts.SizeUpContext.Cities.AsQueryable();
            foreach (var qs in term.Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(qs))
                {
                    keywords = keywords.Where(i => i.Name.Contains(qs));
                }
            }
            keywords = keywords.OrderBy(i => i.Name);
            keywords = keywords.Take(maxResults);
            var data = keywords.Select(i => new
            {
                i.Id,
                i.Name
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
