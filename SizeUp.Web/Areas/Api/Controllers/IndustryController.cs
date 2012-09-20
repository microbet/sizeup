using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core.DataAccess;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class IndustryController : Controller
    {
        //
        // GET: /Api/Industry/
        public JsonResult Industry(int? id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var industry = context.Industries.Where(i => i.Id == id);
                var data = industry.Select(i => new Models.Industry.Industry()
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IndustryList(List<long> ids)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var industry = context.Industries.Where(i => ids.Contains(i.Id));
                var data = industry.Select(i => new Models.Industry.Industry()
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CurrentIndustry()
        {
            var id = SizeUp.Core.Web.WebContext.Current.CurrentIndustryId;
            using (var context = ContextFactory.SizeUpContext)
            {
                var industry = context.Industries.Where(i => i.Id == id);
                var data = industry.Select(i => new Models.Industry.Industry()
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CurrentIndustry(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var c = context.Industries.Where(i => i.Id == id).FirstOrDefault();
                if (c != null)
                {
                    WebContext.Current.CurrentIndustryId = id;
                }
                return Json(c!=null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchIndustries(string term, int maxResults = 35)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var searchSpace = context.Industries
                    .Where(i=>i.IsActive)
                    .Select(i=> new
                    {
                        Id = i.Id,
                        i.Name,
                        i.SEOKey,
                        SortOrder = 1
                    }).Concat(context.IndustryKeywords.Select(i => new
                    {
                        Id = i.IndustryId,
                        i.Name,
                        i.Industry.SEOKey,
                        i.SortOrder
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
                    .Select(i => new Models.Industry.Industry()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult HasData(int id, int placeId)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var location = Locations.Get(context, placeId).FirstOrDefault();
                var rev = IndustryData.GetCity(context, id, location.City.Id).Select(i => i.AverageRevenue).FirstOrDefault();
                return Json(rev.HasValue, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
