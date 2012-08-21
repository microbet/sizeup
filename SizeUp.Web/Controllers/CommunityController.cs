using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using SizeUp.Web.Models;
using SizeUp.Core.DataAccess;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using SizeUp.Core;

namespace SizeUp.Web.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Community/

        public ActionResult CommunityWithIndustry()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var location = Locations.Get(context, WebContext.Current.CurrentPlaceId.Value).FirstOrDefault();
                ViewBag.BusinessCount = BusinessData.GetByCity(context, WebContext.Current.CurrentIndustryId.Value, location.City.Id).Count();
                return View();
            }
        }


        public ActionResult Community()
        {

            return View();
        }

        public ActionResult Find()
        {

            using (var context = ContextFactory.SizeUpContext)
            {
                int pagesize = 21;
                string stateId = Request["stateId"];
                string industryId = Request["industryId"];
                string page = Request["page"];
                ViewBag.States = context.States.Select(i => new Option<long>{ Id = i.Id, Name = i.Name }).ToList();

                var results = IndustryData.GetCities(context)
                    .Join(context.CityCountyMappings, i => i.CityId, o => o.CityId, (i, o) => new { Place = o, i.Industry })
                    .Where(i => i.Industry != null);

                int p = 0;
                if (!string.IsNullOrWhiteSpace(page))
                {
                    p = int.Parse(page);
                }

                if (!string.IsNullOrWhiteSpace(stateId))
                {
                    var id = long.Parse(stateId);
                    results = results.Where(i => i.Place.County.StateId == id);
                }

                if (!string.IsNullOrWhiteSpace(industryId))
                {
                    var id = long.Parse(industryId);
                    results = results.Where(i => i.Industry.Id == id);
                }

                int total = results.Count();

                ViewBag.LastPage = pagesize * (p + 1) >= total;
                ViewBag.FirstPage = p == 0;


                ViewBag.Communities = results
                    .OrderBy(i=>i.Place.Id)
                    .ThenBy(i=>i.Industry.Id)
                    .Skip(pagesize * p)
                    .Take(pagesize)
                    .Select(i => new Models.Community.Community()
                    {
                        City = i.Place.City.Name,
                        County = i.Place.County.Name,
                        State = i.Place.County.State.Abbreviation,
                        Industry = i.Industry.Name,
                        CitySEO = i.Place.City.SEOKey,
                        CountySEO = i.Place.County.SEOKey,
                        StateSEO = i.Place.County.State.SEOKey,
                        IndustrySEO = i.Industry.SEOKey
                    })
                    .ToList()
                    .InSetsOf(pagesize / 3).ToList();

                var prev = new NameValueCollection(Request.QueryString);
                var next = new NameValueCollection(Request.QueryString);

                prev["page"] = (p - 1).ToString();
                next["page"] = (p + 1).ToString(); ;

                ViewBag.Prev = string.Join("&", Array.ConvertAll(prev.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(prev[key]))));
                ViewBag.Next = string.Join("&", Array.ConvertAll(next.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(next[key])))); ;

                return View();
            }
        }


        public ActionResult RedirectWithIndustry(string oldSEO, string industry)
        {
            return View();
        }

        public ActionResult Redirect(string oldSEO)
        {
            return View();
        }


    }
}
