using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
           

            var results = DataContexts.SizeUpContext.Businesses.AsQueryable();


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
                city = DataContexts.SizeUpContext.Cities.Where(i => i.Id == id).FirstOrDefault();
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

            results = results.OrderBy(i => i.Name).Skip(pagesize * p).Take(pagesize);
            results = results.Where(i => i.ZipCode != null && i.Industry != null);
            var data = results.Select(i=> new Models.Business.BusinessItem(){ Business = i, State = i.State, Industry = i.Industry, ZipCode = i.ZipCode }).ToList();
            ViewBag.Businesses = data.InSetsOf(pagesize/3).ToList();


            return View();
        }

    }
}
