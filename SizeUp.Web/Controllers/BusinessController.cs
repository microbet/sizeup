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
                var results = context.Businesses.Where(i=>i.IndustryId != null && i.ZipCodeId !=null && i.CountyId != null && (i.BusinessStatusCode != "1" || i.BusinessStatusCode != "3"));

                var mappings = context.BusinessCityMappings
                    .Join(context.CityCountyMappings, i => new { City = i.CityId, County = i.Business.CountyId.Value }, o => new { City = o.CityId, County = o.CountyId }, (i, o) => new { i.BusinessId, o.Id });

                
              

                if (!string.IsNullOrWhiteSpace(industryId))
                {
                    int id = int.Parse(industryId);
                    results = results.Where(i => i.IndustryId == id);
                }

                if (!string.IsNullOrWhiteSpace(placeId))
                {
                    int id = int.Parse(placeId);
                    results = results.Join(mappings, i => i.Id, o => o.BusinessId, (i, o) => new { business = i, mapping = o })
                                .Where(i => i.mapping.Id == id)
                                .Select(i => i.business);
                }

                if (!string.IsNullOrWhiteSpace(businessName))
                {
                    results = results.Where(i => i.Name.StartsWith(businessName));
                }

                int p = 0;
                if (!string.IsNullOrWhiteSpace(page))
                {
                    p = int.Parse(page);
                }

                results = results.OrderBy(i => i.Name);
                var total = results.Count();
                ViewBag.LastPage = pagesize * (p + 1) >= total;
                ViewBag.FirstPage = p == 0;


                results = results.Skip(pagesize * p).Take(pagesize);
                var data = results.Select(i => new Models.Business.BusinessResult()
                { 
                    Name = i.Name,
                    Address = i.Address,
                    City = i.City,
                    State = i.State.Abbreviation,
                    Zip = i.ZipCode.Zip,
                    StateSEO = i.State.SEOKey,
                    CountySEO = i.County.SEOKey,
                    IndustrySEO = i.Industry.SEOKey
                    
                    
                    //,
                    //CitySEO = i.BusinessCityMappings.
                   
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

    }
}
