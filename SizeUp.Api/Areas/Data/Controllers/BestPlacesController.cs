﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class BestPlacesController : BaseController
    {
        //
        // GET: /Api/TopPlaces/
        
        private BestPlacesFilters BuildFilters()
        {
            BestPlacesFilters f = new BestPlacesFilters();
            f.AverageRevenue = ParseQueryString("averageRevenue");
            f.TotalRevenue = ParseQueryString("totalRevenue");
            f.TotalEmployees = ParseQueryString("totalEmployees");
            f.RevenuePerCapita = ParseQueryString("revenuePerCapita");
            f.HouseholdIncome = ParseQueryString("householdIncome");
            f.HouseholdExpenditures = ParseQueryString("householdExpenditures");
            f.MedianAge = ParseQueryString("medianAge");
            f.BachelorOrHigher = QueryString.IntValue("bachelorsDegreeOrHigher");
            f.HighSchoolOrHigher = QueryString.IntValue("highSchoolOrHigher");
            f.WhiteCollarWorkers = QueryString.IntValue("whiteCollarWorkers");
            f.BlueCollarWorkers = QueryString.IntValue("blueCollarWorkers");
            f.AirportsNearby = QueryString.IntValue("airportsNearby");
            f.YoungEducated = QueryString.IntValue("youngEducated");
            f.UniversitiesNearby = QueryString.IntValue("universitiesNearby");
            f.CommuteTime = QueryString.IntValue("commuteTime");
            f.Attribute = QueryString.StringValue("attribute");
            return f;
        }


        private Band<int?> ParseQueryString(string index)
        {
            Band<int?> v = null;
            int?[] ar = QueryString.IntValues(index);

            if (ar != null)
            {
                v = new Band<int?>();
                v.Min = ar[0];
                v.Max = ar[1];
            }
            return v;
        }

        
        [APIAuthorize(Role = "BestPlaces")]
        public ActionResult Index(int industryId, Core.DataLayer.Granularity granularity, long? regionId, long? stateId, int itemCount = 25)
        {
            int maxResults = int.Parse(ConfigurationManager.AppSettings["Data.BestPlaces.MaxResults"]);
            itemCount = Math.Min(maxResults, itemCount);
            BestPlacesFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.Get(context, industryId, regionId, stateId, filters, granularity).Take(itemCount).ToList();
                if (granularity == Core.DataLayer.Granularity.City)
                {
                    var cityIds = output.Select(i=>i.City.Id).ToList();
                    var counties = context.Cities.Where(c => cityIds.Contains(c.Id)).SelectMany(i => i.Counties, (i, o) => new { i.Id, o.Name }).ToList();
                    output.ForEach(i => i.City.Counties = counties.Where(c => c.Id == i.City.Id).Select(c => new Core.DataLayer.Models.County { Name = c.Name }).ToList());
                }
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


        [APIAuthorize(Role = "BestPlaces")]
        public ActionResult Bands(int itemCount, int bands, int industryId, Core.DataLayer.Granularity granularity, long? regionId, long? stateId)
        {
            BestPlacesFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.Bands(context, industryId, itemCount, bands, regionId, stateId, filters, granularity);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }

        /*
        [APIAuthorize(Role = "BestPlaces")]
        public ActionResult IndustryRanks(int rankCutoff, long placeId, Granularity granularity)
        {
            BestPlacesFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.IndustryRanks(context, rankCutoff, placeId, granularity);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }*/
    }
}
