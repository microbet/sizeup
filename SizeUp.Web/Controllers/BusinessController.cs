using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using SizeUp.Data;
using SizeUp.Web.Models.Business;
using SizeUp.Core.Extensions;


namespace SizeUp.Web.Controllers
{
    public class BusinessController : BaseController
    {
        //
        // GET: /Business/

        public ActionResult Find()
        {
            int pagesize = 21;
            string industryId = Request["industryId"];
            string placeId = Request["placeId"];
            string businessName = Request["name"];
            string page = Request["page"];

            using (var context = ContextFactory.SizeUpContext)
            {
                var mappings = context.BusinessCityMappings
                  .Join(context.CityCountyMappings, i => new { City = i.CityId, County = i.Business.CountyId.Value }, o => new { City = o.CityId, County = o.CountyId }, (i, o) => new { i.Business, Place = o });


                var results = mappings
                    .Join(context.Businesses, i => i.Business.Id, o => o.Id, (i, o) => new { Business = o, Place = i.Place })
                    .Where(i => i.Business.IsActive);
      
           
              
                
              

                if (!string.IsNullOrWhiteSpace(industryId))
                {
                    int id = int.Parse(industryId);
                    results = results.Where(i => i.Business.IndustryId == id);
                }

                if (!string.IsNullOrWhiteSpace(placeId))
                {
                    int id = int.Parse(placeId);
                    results = results.Where(i => i.Place.Id == id);
                }

                if (!string.IsNullOrWhiteSpace(businessName))
                {
                    results = results.Where(i => i.Business.Name.StartsWith(businessName));
                }

                int p = 0;
                if (!string.IsNullOrWhiteSpace(page))
                {
                    p = int.Parse(page);
                }

                results = results.OrderBy(i => i.Business.Name);
                var total = results.Count();
                ViewBag.LastPage = pagesize * (p + 1) >= total;
                ViewBag.FirstPage = p == 0;


                results = results.Skip(pagesize * p).Take(pagesize);
                var data = results.Select(i => new Models.Business.BusinessResult()
                {
                    Id = i.Business.Id,
                    Name = i.Business.Name,
                    Address = i.Business.Address,
                    City = i.Business.City,
                    State = i.Business.State.Abbreviation,
                    Zip = i.Business.ZipCode.Zip,
                    StateSEO = i.Business.State.SEOKey,
                    CountySEO = i.Business.County.SEOKey,
                    CitySEO = i.Place.City.SEOKey,
                    IndustrySEO = i.Business.Industry.SEOKey,
                    SEOKey = i.Business.SEOKey
                   
                }).ToList();

                ViewBag.Businesses = data.InSetsOf(pagesize / 3).ToList();

                var prev = new NameValueCollection(Request.QueryString);
                var next = new NameValueCollection(Request.QueryString);

                prev["page"] = (p - 1).ToString();
                next["page"] = (p + 1).ToString(); ;

                ViewBag.Prev = string.Join("&", Array.ConvertAll(prev.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(prev[key]))));
                ViewBag.Next = string.Join("&", Array.ConvertAll(next.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(next[key])))); ;
                return View();
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
                        Zip = i.ZipCode.Zip,
                        IsPublic = i.PublicCompanyIndicator == "1",
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
