using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class CityController : Controller
    {
        //
        // GET: /Api/City/

        public JsonResult City(int? id)
        {
            var item = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id);
            var data = item.Select(i => new
            {
                i.Id,
                i.Name,
                County = i.County.Name,
                State = i.State.Abbreviation,
                FullName = i.Name + ", " + i.State.Abbreviation,
                i.SEOKey
                
            }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CurrentCity()
        {
            var item = SizeUp.Core.Web.WebContext.Current.CurrentCity;
            object data = null;
            if (item != null)
            {
                data = new
                {
                    item.Id,
                    item.Name,
                    County = item.County.Name,
                    State = item.State.Abbreviation,
                    FullName = item.Name + ", " + item.State.Abbreviation,
                    item.SEOKey
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CurrentCity(int id)
        {
            var c = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id).FirstOrDefault();
            WebContext.Current.CurrentCity = c;
            return Json(c!=null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetectedCity()
        {
            var item = SizeUp.Core.Web.WebContext.Current.DetectedCity;
            object data = null;
            if (item != null)
            {
                data = new
                {
                    item.Id,
                    item.Name,
                    County = item.County.Name,
                    State = item.State.Abbreviation,
                    FullName = item.Name + ", " + item.State.Abbreviation,
                    item.SEOKey
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCities(string term, int maxResults = 35)
        {
            var keywords = DataContexts.SizeUpContext.Cities.AsQueryable();
            var data = keywords.Select(i => new
            {
                i.Id,
                i.Name,
                County = i.County.Name,
                State = i.State.Abbreviation,
                FullName = i.Name + ", " + i.State.Abbreviation,
                i.SEOKey
            });

            data = data.Where(i => i.FullName.StartsWith(term));
            data = data.OrderBy(i => i.Name);
            data = data.Take(maxResults);
            var list = data.ToList();

            var output = list.Select(i => new 
            {
                i.Id,
                i.Name,
                i.County,
                i.State,
                i.FullName,
                i.SEOKey,
                DisplayName = list.Count(s => s.FullName == i.FullName) > 1 ? i.FullName + string.Format(" ({0})", i.County) : i.FullName

            });
            return Json(output, JsonRequestBehavior.AllowGet);
        }
    }
}
