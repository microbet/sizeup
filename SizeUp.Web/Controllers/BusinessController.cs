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
            string cityId = Request["cityId"];
            string businessName = Request["name"];
            string page = Request["page"];

            using (var context = ContextFactory.SizeUpContext)
            {
                var results = context.Businesses.AsQueryable();


                Data.Industry industry = null;
                Data.City city = null;

                if (!string.IsNullOrWhiteSpace(industryId))
                {
                    int id = int.Parse(industryId);
                    results = results.Where(i => i.IndustryId == id);
                }

                if (!string.IsNullOrWhiteSpace(cityId))
                {
                    int id = int.Parse(cityId);
                    city = context.Cities.Where(i => i.Id == id).FirstOrDefault();
                    results = results.Where(i => city.Geography.Intersects(i.Geography));
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

                results = results.Where(i => i.ZipCode != null && i.Industry != null).OrderBy(i => i.Name);
                //gotta fix this the counts are killing us
                //also when we do a search on just a city we get creamed
                var total = results.Count();
                ViewBag.LastPage = pagesize * (p + 1) >= total;
                ViewBag.FirstPage = p == 0;


                results = results.Skip(pagesize * p).Take(pagesize);
                var data = results.Select(i => new Models.Business.BusinessItem() { Business = i, State = i.State, Industry = i.Industry, ZipCode = i.ZipCode }).ToList();
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
