using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Core;
using SizeUp.Core.Serialization;



namespace SizeUp.Web.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Community/

        public ActionResult CommunityWithIndustry()
        {
            if (CurrentInfo.CurrentPlace.Id == null || CurrentInfo.CurrentIndustry == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                var c= Core.DataLayer.Business.ListIn(context, WebContext.Current.CurrentIndustry.Id, WebContext.Current.CurrentPlace.Id.Value).Count();
                if (c == 0)
                {
                    throw new HttpException(404, "Page Not Found");
                }
                ViewBag.BusinessCount = c;
                return View();
            }
        }


        public ActionResult CityCommunity()
        {
            return View();     
        }

        public ActionResult CountyCommunity(string state, string county) 
        {
            if (CurrentInfo.CurrentPlace.State.Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            ActionResult action = View();
            using (var context = ContextFactory.SizeUpContext)
            {
                if (CurrentInfo.CurrentPlace.State.Id == null)
                {
                    //hack becuase we have route collisions
                    action = RedirectWithIndustry(state, county);
                }
                return action;
            }
        }

        public ActionResult MetroCommunity(string metro)
        {
            if (CurrentInfo.CurrentPlace.Metro.Id == null)
            {
                throw new HttpException(404, "Page Not Found");
            }
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.CurrentInfoJSON = Serializer.ToJSON(CurrentInfo);
                return View();
            }
        }

        public ActionResult StateCommunity(string state)
        {
            if (CurrentInfo.CurrentPlace.State.Id == null )
            {
                throw new HttpException(404, "Page Not Found");
            }
            ActionResult action = View();
            using (var context = ContextFactory.SizeUpContext)
            {
                if (CurrentInfo.CurrentPlace.State.Id == null)
                {
                    //hack becuase we have route collisions
                    action = Redirect(state);
                }              
                return action;
            }
        }





        public ActionResult FindState()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                ViewBag.States = Core.DataLayer.State.List(context)
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
                            d.BusinessCount > 0 &&
                            d.Industry.IsActive && !d.Industry.IsDisabled))
                    .Select(new Core.DataLayer.Projections.Place.Default().Expression);
                
               

                var groups = places
                    .ToList()
                    .OrderBy(i=>i.City.Name)
                    .GroupBy(i=>i.City.Name.Substring(0,1));

                ViewBag.Cities = groups
                    .Select(i=> i.ToList())
                    .InSetsOf((int)System.Math.Ceiling(groups.Count() / 2d))
                    .ToList();
                return View("City");
            }
        }


        public ActionResult FindIndustry(string state, string county, string city)
        {
            if (CurrentInfo.CurrentPlace.Id == null )
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





        public ActionResult RedirectWithIndustry(string oldSEO, string industry)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = Core.DataLayer.Place.GetLegacy(context, oldSEO);
                var ind = Core.DataLayer.Industry.GetLegacy(context, industry).SEOKey;

                if (place == null)
                {
                    throw new HttpException(404, "Page Not Found");
                }

                string url = string.Format("/community/{0}/{1}/{2}/{3}", place.State.SEOKey,place.County.SEOKey,place.City.SEOKey, ind);
                return RedirectPermanent(url);
            }
        }

        public ActionResult Redirect(string oldSEO)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var place = Core.DataLayer.Place.GetLegacy(context, oldSEO);


                if (place == null)
                {
                    throw new HttpException(404, "Page Not Found");
                }
                else
                {
                    string url = string.Format("/community/{0}/{1}/{2}", place.State.SEOKey, place.County.SEOKey, place.City.SEOKey);
                    return RedirectPermanent(url);
                }
            }
        }


    }
}
