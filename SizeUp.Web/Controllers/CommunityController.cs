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




        public ActionResult FindState()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.States = context.States
                    .Select(i => new Models.Business.State()
                    {
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    })
                    .OrderBy(i => i.Name)
                    .ToList()
                    .InSetsOf(14);

                return View("State");
            }
        }


        public ActionResult FindCity(string state)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var s = context.States
                    .Select(i => new Models.Business.State()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    })
                    .Where(i => i.SEOKey == state)
                    .FirstOrDefault();

                ViewBag.State = s;


                var data = context.CityCountyMappings
                    .Where(i => i.County.StateId == s.Id && i.City.CityType.IsActive)
                    .Select(i => new Models.Business.Place()
                    {
                        CityName = i.City.Name,
                        CitySEOKey = i.City.SEOKey,
                        CityType = i.City.CityType.Name,
                        CountyName = i.County.Name,
                        CountySEOKey = i.County.SEOKey
                    })
                    .OrderBy(i => i.CityName)
                    .ThenBy(i => i.CountyName)
                    .ToList();

                data.ForEach(i => i.DisplayType = data.Count(p => p.CityName == i.CityName && p.CityName == i.CityName && p.CountyName == i.CountyName) > 1);


                var groups = data
                   .GroupBy(i => i.CityName.Substring(0, 1))
                   .Select(i => new Models.Business.PlaceList()
                   {
                       Key = i.Key,
                       Places = i.ToList()
                   })
                   .ToList();


                ViewBag.Cities = groups.InSetsOf((int)System.Math.Ceiling(groups.Count / 2d))
                    .ToList();
                return View("City");
            }
        }


        public ActionResult FindIndustry(string state, string county, string city)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var location = context.CityCountyMappings
                   .Where(i => i.County.State.SEOKey == state
                       && i.City.CityType.IsActive
                       && i.City.SEOKey == city
                       && i.County.SEOKey == county)
                   .Select(i => new Models.Business.Place()
                   {
                       CityId = i.City.Id,
                       CityName = i.City.Name,
                       CitySEOKey = i.City.SEOKey,
                       CountyName = i.County.Name,
                       CountySEOKey = i.County.SEOKey,
                       CityType = i.City.CityType.Name,
                       StateName = i.County.State.Name,
                       StateSEOKey = i.County.State.SEOKey
                   })
                   .FirstOrDefault();

                ViewBag.Place = location;

                var industries = context.Industries
                    .Where(i => i.IsActive)
                    .Where(i=>i.IndustryDataByCities.Any(m=>m.CityId == location.CityId))
                    .Join(context.Industries, i => i.SicCode.Substring(0, 4), o => o.SicCode, (i, o) => new { Industry = i, Parent = o })
                    .ToList()
                    .GroupBy(i => i.Parent)
                    .Select(i => new Models.Business.IndustryList()
                    {
                        Key = i.Key.Name,
                        Industries = i.Select(o => new Models.Business.Industry()
                        {
                            Name = o.Industry.Name,
                            SEOKey = o.Industry.SEOKey
                        }).OrderBy(o => o.Name).ToList()
                    })
                    .OrderBy(i => i.Key)
                    .ToList();



                ViewBag.Industries = industries
                    .InSetsOf((int)System.Math.Ceiling(industries.Count() / 2d))
                    .ToList();


                return View("Industry");
            }
        }





        public ActionResult RedirectWithIndustry(string oldSEO, string industry)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = context.LegacyCommunitySEOKeys.Where(i => i.SEOKey == oldSEO)
                    .Select(i => new
                    {
                        State = i.City.State.SEOKey,
                        County = i.City.CityCountyMappings.FirstOrDefault().County.SEOKey,
                        City = i.City.SEOKey
                    })
                    .FirstOrDefault();

                var ind = context.LegacyIndustrySEOKeys.Where(i => i.SEOKey == industry)
                    .Select(i => i.Industry.SEOKey)
                    .FirstOrDefault();

                if (place == null)
                {
                    throw new HttpException(404, "Page Not Found");
                }

                string url = string.Format("/community/{0}/{1}/{2}/{3}", place.State,place.County,place.City, ind);
                return RedirectPermanent(url);
            }
        }

        public ActionResult Redirect(string oldSEO)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = context.LegacyCommunitySEOKeys.Where(i => i.SEOKey == oldSEO)
                 .Select(i => new
                 {
                     State = i.City.State.SEOKey,
                     County = i.City.CityCountyMappings.FirstOrDefault().County.SEOKey,
                     City = i.City.SEOKey
                 })
                 .FirstOrDefault();


                if (place == null)
                {
                    throw new HttpException(404, "Page Not Found");
                }
                else
                {
                    string url = string.Format("/community/{0}/{1}/{2}", place.State, place.County, place.City);
                    return RedirectPermanent(url);
                }
            }
        }


    }
}
