using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
namespace SizeUp.Web.Controllers
{
    public class SitemapController : Controller
    {
        //
        // GET: /Sitemap/

        public ActionResult Index()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.BusinessCount = Core.DataLayer.Business.Get(context).Count(i=>i.Industry.IsActive && !i.Industry.IsDisabled);
                ViewBag.CommunityCount = Core.DataLayer.Place.Get(context).Count(); 
                ViewBag.IndustryCommunityCount = Core.DataLayer.IndustryData.Get(context).Count(i=>i.GeographicLocation.City.IsActive && i.GeographicLocation.City.CityType.IsActive);

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult Business(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = Core.DataLayer.Business.Get(context)
                    .Where(i=>i.Industry.IsActive && !i.Industry.IsDisabled)
                    .Select(i => new Core.DataLayer.Models.Sitemap.Business
                    {
                        Id = i.Id,
                        SEOKey = i.SEOKey,
                        Industry = i.Industry.SEOKey,
                        City = i.Cities.Select(c => c.SEOKey).FirstOrDefault(),
                        County = i.County.SEOKey,
                        State = i.County.State.SEOKey
                    })
                    .OrderBy(i => i.Id)
                    .Skip(index)
                    .Take(50000)
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult Community()
        {
            Response.ContentType = "text/xml";
            return View();
        }

        public ActionResult CommunityIndustry()
        {
            Response.ContentType = "text/xml";
            return View();
        }

    }
}
