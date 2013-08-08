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
                ViewBag.BusinessCount = context.SitemapBusinesses.Count();
                ViewBag.CommunityCount = context.SitemapCommunities.Count();
                ViewBag.CommunityIndustryCount = context.SitemapCommunityIndustries.Count();

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult Business(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = context.SitemapBusinesses
                    .Where(i=>i.Id>index)
                    .OrderBy(i => i.Id)
                    .Take(50000)
                    .Select(i => new Core.DataLayer.Models.Sitemap.Business
                    {
                        Id = i.BusinessId,
                        SEOKey = i.Business,
                        Industry = i.Industry,
                        City = i.City,
                        County = i.County,
                        State = i.State
                    })
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult Community(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = context.SitemapCommunities
                    .Where(i => i.Id > index)
                    .OrderBy(i => i.Id)
                    .Take(50000)
                    .Select(i => new Core.DataLayer.Models.Sitemap.Community
                    {
                        City = i.City,
                        County = i.County,
                        State = i.State
                    })
                    .ToList();


                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult CommunityIndustry(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = context.SitemapCommunityIndustries
                    .Where(i => i.Id > index)
                    .OrderBy(i => i.Id)
                    .Take(50000)
                    .Select(i => new Core.DataLayer.Models.Sitemap.CommunityIndustry
                    {
                        Industry = i.Industry,
                        City = i.City,
                        County = i.County,
                        State = i.State
                    })
                    .ToList();


                Response.ContentType = "text/xml";
                return View();
            }
        }

    }
}
