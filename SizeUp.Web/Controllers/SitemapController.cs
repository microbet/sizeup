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
                ViewBag.SiteMapMainCount = context.SitemapMains.Count();
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
                    .Join(context.Industries, x => x.Industry, i => i.SEOKey, (x, i) => new { Industry = i, SitemapCommunityIndustry = x })
                    .Where(i=>i.Industry.IsActive == true && i.Industry.IsDisabled == false )
                    .Select(i => new Core.DataLayer.Models.Sitemap.CommunityIndustry
                    {
                        Industry = i.SitemapCommunityIndustry.Industry,
                        City = i.SitemapCommunityIndustry.City,
                        County = i.SitemapCommunityIndustry.County,
                        State = i.SitemapCommunityIndustry.State
                    })                   
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult Advertising(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = context.SitemapMains
                    .Where(i => i.Id > index)
                    .OrderBy(i => i.Id)
                    .Take(50000)
                    .Join(context.Industries, x => x.Industry, i => i.SEOKey, (x, i) => new { Industry = i, Main = x })
                    .Where(i => i.Industry.IsActive == true && i.Industry.IsDisabled == false)
                    .Select(i => new Core.DataLayer.Models.Sitemap.Advertising
                    {
                        Industry = i.Main.Industry,
                        City = i.Main.City,
                        County = i.Main.County,
                        State = i.Main.State
                    })
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }
    }
}
