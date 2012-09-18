using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using SizeUp.Data;
using SizeUp.Web.Models.Business;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataAccess;

namespace SizeUp.Web.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Business/

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
                    .OrderBy(i=>i.Name)
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
                    .OrderBy(i=>i.CityName)
                    .ThenBy(i=>i.CountyName)
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
                   .Select(i=> new Models.Business.Place(){
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
                    .Where(i=> i.Businesses.Any(b=>b.BusinessCityMappings.Any(m=>m.CityId == location.CityId) && b.IsActive))
                    .Join(context.Industries, o => o.SicCode.Substring(0, 4), i => i.SicCode, (i, o) => new { Industry = i, Parent = o })
                    .ToList()
                    .GroupBy(i => i.Parent)
                    .Select(i => new IndustryList()
                    {
                        Key = i.Key.Name,
                        Industries = i.Select(o => new Models.Business.Industry()
                        {
                            Name = o.Industry.Name,
                            SEOKey = o.Industry.SEOKey
                        }).OrderBy(o=>o.Name).ToList()
                    })
                    .OrderBy(i=>i.Key)
                    .ToList();
                    


                ViewBag.Industries = industries
                    .InSetsOf((int)System.Math.Ceiling(industries.Count() / 2d))
                    .ToList();
     

                return View("Industry");
            }
        }




        public ActionResult Find(string state, string county, string city, string industry)
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

                var ind = context.Industries
                    .Where(i => i.SEOKey == industry && i.SicCode.Length == 6)
                    .Select(i => new Models.Business.Industry()
                    {
                        IndustryId = i.Id,  
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    })
                    .FirstOrDefault();

                ViewBag.Industry = ind;


                var businesses = context.Businesses
                    .Where(i => i.BusinessCityMappings.Any(m => m.CityId == location.CityId) && i.IndustryId == ind.IndustryId && i.IsActive)
                    .Select(i => new Models.Business.BusinessResult()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Address = i.Address,
                        City = i.City,
                        State = i.State.Abbreviation,
                        Zip = i.ZipCode.Zip,
                        SEOKey = i.SEOKey
                    })
                    .ToList();


                ViewBag.Businesses = businesses
                    .OrderBy(i => i.Name)
                    .InSetsOf((int)System.Math.Ceiling(businesses.Count() / 2d))
                    .ToList();







                return View("Find");
            }
        }


        public ActionResult Business(string state, string county, string city, string industry, string name, long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var business = context.Businesses.Where(i => i.Id == id && i.SEOKey == name && i.State.SEOKey == state && i.County.SEOKey == county && i.Industry.SEOKey == industry)
                    .Select(i=> new Models.Business.BusinessResult()
                    {
                        Name = i.Name,
                        Address = i.Address,
                        City = i.City,
                        State = i.State.Abbreviation,
                        Phone = i.Phone,
                        WebSite = i.PrimaryWebURL,
                        Zip = i.ZipCode.Zip,
                        IsPublic = i.PublicCompanyIndicator == "1" || i.PublicCompanyIndicator == "2",
                        Lat = i.Lat.Value,
                        Long = i.Long.Value
                    }).FirstOrDefault();

                if (business != null)
                {
                    ViewBag.Business = business;
                }
                else
                {
                    throw new HttpException(404, "Page Not Found"); 
                }
                return View();
            }
        }
    }
}
