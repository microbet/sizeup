using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;

namespace SizeUp.Web.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/

        public ActionResult Index(string state, string city, string industry)
        {
            var ind = DataContexts.SizeUpContext.Industries.Where(i => i.SEOKey == industry)
                .Select(i=> new {Industry = i, NAICS6 = DataContexts.SizeUpContext.SicToNAICSMappings.Where(m=>m.IndustryId == i.Id).Select(m=>m.NAICS).FirstOrDefault()})
                .FirstOrDefault();
            var s = DataContexts.SizeUpContext.States.Where(i => i.Abbreviation == state).FirstOrDefault();
            var c = DataContexts.SizeUpContext.Cities.Where(i => i.SEOKey == city && i.StateId == s.Id).Select(i=> new {City = i, County = i.County, Metro = i.County.Metro}).FirstOrDefault();
            WebContext.Current.CurrentCity = c.City;
            WebContext.Current.CurrentIndustry = ind.Industry;
            ViewBag.Industry = WebContext.Current.CurrentIndustry;
            ViewBag.NAICS6 = ind.NAICS6;
            ViewBag.City = c.City;
            ViewBag.State = s;

            if (c.Metro != null)
            {
                ViewBag.IdsJSON = SizeUp.Core.Serialization.Serializer.ToJSON(new Models.Business.ReportIds()
                {
                    industryId = ind.Industry.Id,
                    cityId = c.City.Id,
                    countyId = c.County.Id,
                    stateId = s.Id,
                    metroId = c.Metro.Id
                });
            }
            else
            {
                ViewBag.IdsJSON = SizeUp.Core.Serialization.Serializer.ToJSON(new Models.Business.ReportIds()
                {
                    industryId = ind.Industry.Id,
                    cityId = c.City.Id,
                    countyId = c.County.Id,
                    stateId = s.Id,
                });
            }

            ViewBag.Header.ActiveTab = NavItems.Dashboard;
            return View();
        }

    }
}
