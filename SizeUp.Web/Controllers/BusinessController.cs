using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using SizeUp.Data;
using SizeUp.Core.Extensions;

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
                ViewBag.States = Core.DataLayer.Place.List(context)
                    .Select(i => i.State)
                    .Distinct()
                    .OrderBy(i => i.Name)
                    .ToList()
                    .InSetsOf(14);
                return View("State");
            }
        }


        public ActionResult FindCity(string state)
        {
            if (CurrentInfo.CurrentPlace.State.Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }

            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.State = CurrentInfo.CurrentPlace.State;



                var places = Core.DataLayer.Place.Get(context)
                    .Where(i => i.City.StateId == CurrentInfo.CurrentPlace.State.Id.Value)
                    .Where(i =>
                        i.City.GeographicLocation.IndustryDatas
                        .Any(d =>
                            d.Year == Core.DataLayer.CommonFilters.TimeSlice.Industry.Year &&
                            d.Quarter == Core.DataLayer.CommonFilters.TimeSlice.Industry.Quarter &&
                            d.Industry.IsActive && !d.Industry.IsDisabled))
                    .Select(new Core.DataLayer.Projections.Place.Default().Expression);



                var groups = places
                    .ToList()
                    .OrderBy(i => i.City.Name)
                    .GroupBy(i => i.City.Name.Substring(0, 1));

                ViewBag.Cities = groups
                    .Select(i => i.ToList())
                    .InSetsOf((int)System.Math.Ceiling(groups.Count() / 2d))
                    .ToList();
                return View("City");
            }
        }


        public ActionResult FindIndustry(string state, string county, string city)
        {
            if (CurrentInfo.CurrentPlace.Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var industries = Core.DataLayer.Industry.ListInPlace(context, CurrentInfo.CurrentPlace.Id.Value)
                    .ToList()
                    .OrderBy(i => i.ParentName)
                    .GroupBy(i => i.ParentName)
                    .ToList();

                ViewBag.Industries = industries
                    .Select(i => i.ToList())
                    .InSetsOf((int)System.Math.Ceiling(industries.Count() / 2d))
                    .ToList();


                return View("Industry");
            }
        }



        public ActionResult Find(string state, string county, string city, string industry)
        {
            if (CurrentInfo.CurrentPlace.Id == null || CurrentInfo.CurrentIndustry.Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var businesses = Core.DataLayer.Business.ListIn(context, Core.Web.WebContext.Current.CurrentIndustry.Id, Core.Web.WebContext.Current.CurrentPlace.Id.Value);
                ViewBag.Businesses = businesses
                    .OrderBy(i => i.Name)
                    .InSetsOf((int)System.Math.Ceiling(businesses.Count() / 2d))
                    .ToList();
                return View("Find");
            }
        }


        public ActionResult Business(string state, string county, string city, string industry, string name, long id)
        {
            if (CurrentInfo.CurrentPlace.Id == null || CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var business = Core.DataLayer.Business.GetIn(context, id, Core.Web.WebContext.Current.CurrentPlace.Id.Value);
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



        public ActionResult Redirect(string oldSEO)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var business = Core.DataLayer.Business.GetLegacy(context, oldSEO);
                var place = Core.DataLayer.Place.GetByBusiness(context, business != null ? business.Id.Value : 0);
                var industry = Core.DataLayer.Industry.Get(context, business != null ? business.IndustryId : 0);

                if (place == null || business == null || industry == null)
                {
                    throw new HttpException(404, "Page Not found");
                }
                else
                {
                    string url = string.Format("/business/{0}/{1}/{2}/{3}/{4}/{5}", place.State.SEOKey, place.County.SEOKey, place.City.SEOKey, industry.SEOKey, business.Id, business.SEOKey);
                    return RedirectPermanent(url);
                }
            }
        }



    }
}
