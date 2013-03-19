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
using SizeUp.Core.DataLayer.Base;


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
                ViewBag.BusinessCount = Core.DataLayer.Business.CountIn(context, WebContext.Current.CurrentIndustryId.Value, WebContext.Current.CurrentPlaceId.Value);
                return View();
            }
        }


        public ActionResult CityCommunity()
        {
            return View();     
        }

        public ActionResult CountyCommunity(string state, string county) 
        {
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
            return View();
        }

        public ActionResult StateCommunity(string state)
        {
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
                ViewBag.States = Core.DataLayer.State.Get(context)
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

                var places = Core.DataLayer.Place.List(context).Where(i=>i.State.Id == CurrentInfo.CurrentPlace.State.Id.Value);
                var industryData = Core.DataLayer.Base.IndustryData.City(context);

                var data = places.Where(i => industryData.Any(d => d.CityId == i.City.Id)).ToList();

                data.ForEach(i => i.DisplayName = data.Count(s => s.City.Name == i.City.Name && s.County.Name == i.County.Name) > 1 ? (i.County.Name + " County - " + i.City.TypeName) : (i.County.Name + " County"));
                var groups = data
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
