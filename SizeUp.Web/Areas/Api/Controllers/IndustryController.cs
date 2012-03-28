using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;


namespace SizeUp.Web2.Areas.Api.Controllers
{
    public class IndustryController : Controller
    {
        //
        // GET: /Api/Industry/


        public JsonResult GetIndustry(int? id)
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

        public JsonResult GetCurrentIndustry()
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
    }
}
