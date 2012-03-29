using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;

namespace SizeUp.Web2.Areas.Api.Controllers
{
    public class IndustryController : Controller
    {
        //
        // GET: /Api/Industry/


        public JsonResult Industry(int? id)
        {
            var industry = DataContexts.SizeUpContext.Industries.Where(i => i.Id == id);
            var data = industry.Select(i => new
            {
                i.Id,
                i.Name,
                i.SEOKey
            }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CurrentIndustry()
        {
            var item = SizeUp.Core.Web.WebContext.Current.CurrentIndustry;
            object data = null;
            if (item != null)
            {
                data = new
                {
                    item.Id,
                    item.Name,
                    item.SEOKey
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CurrentIndustry(int id)
        {
            var c = DataContexts.SizeUpContext.Industries.Where(i => i.Id == id).FirstOrDefault();
            WebContext.Current.CurrentIndustry = c;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchIndustries(string term, int maxResults = 35)
        {
            var keywords = DataContexts.SizeUpContext.IndustryKeywords.AsQueryable();
            var industries = DataContexts.SizeUpContext.Industries.AsQueryable();

            var searchSpace = keywords.Select(i=> new { 
                Id = i.IndustryId ,
                i.Name,
                i.SortOrder
            }).Union(industries.Select(i=> new {
                i.Id,
                i.Name,
                SortOrder = 1
            }));

            foreach (var qs in term.Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(qs))
                {
                    searchSpace = searchSpace.Where(i => i.Name.Contains(qs));
                }
            }

            var data = searchSpace
                .OrderBy(i => i.SortOrder)
                .Take(maxResults)
                .Select(i => new
                {
                    i.Id,
                    i.Name
                });
                
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HasData(int id, int cityId)
        {
            object data = true;
            if (cityId == 3454 && id == 8589)
            {
                data = false;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
