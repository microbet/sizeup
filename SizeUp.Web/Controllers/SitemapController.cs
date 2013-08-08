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
                var pi = Core.DataLayer.Place.Get(context)
                    .SelectMany(i => i.City.Industries.Select(n => new Core.DataLayer.Models.Sitemap.CommunityIndustry
                    {
                        City = i.City.SEOKey,
                        County = i.County.SEOKey,
                        State = i.County.State.SEOKey,
                        Industry = n.SEOKey
                    })).ToList();

                var community = pi
                    .Select(i => new Core.DataLayer.Models.Sitemap.Community
                    {
                        City = i.City,
                        County = i.County,
                        State = i.State
                    }).Distinct().ToList();

                ViewBag.Community = community;
                ViewBag.IndustryCommunity = pi;

                Response.ContentType = "text/xml";
                return View();
            }
        }

        /*
        public ActionResult Index(string state, string county, string city)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = Core.DataLayer.Place.Get(context)
                    .Where(i=>i.County.State.SEOKey == state)
                    .Where(i=>i.County.SEOKey == county)
                    .Where(i=>i.City.SEOKey == city)
                    .SelectMany(i=> i.City.Industries.Select(c=> new { Industry = c, Place = i}))


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

        public ActionResult Index(string state, string county, string city, string industry)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = Core.DataLayer.Business.Get(context)
                    .Where(i => i.Industry.IsActive && !i.Industry.IsDisabled)
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
        */

        /*
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

        public ActionResult Community(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.Data = Core.DataLayer.Place.Get(context)                    
                    .OrderBy(i => i.Id)
                    .Select(i => new Core.DataLayer.Models.Sitemap.Community
                    {
                        City = i.City.SEOKey,
                        County = i.County.SEOKey,
                        State = i.County.State.SEOKey
                    })
                    .Skip(index)
                    .Take(50000)
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }

        public ActionResult CommunityIndustry(int index)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var places = Core.DataLayer.Place.Get(context);
                var industries = Core.DataLayer.Industry.Get(context);

          
                ViewBag.Data = Core.DataLayer.Industry.Get(context)
                    .SelectMany(i=>i.Cities.SelectMany(p=>p.Places.Select(pp=> new { Place = pp, Industry = i})))
                    .OrderBy(i => i.Place.Id)
                    .ThenBy(i=>i.Industry.Id)
                    .Skip(index)
                    .Take(50000)
                    .Select(i => new Core.DataLayer.Models.Sitemap.CommunityIndustry
                    {
                        City = i.Place.City.SEOKey,
                        County = i.Place.County.SEOKey,
                        State = i.Place.County.State.SEOKey,
                        Industry = i.Industry.SEOKey
                    })
                    .ToList();

                Response.ContentType = "text/xml";
                return View();
            }
        }*/

    }
}
