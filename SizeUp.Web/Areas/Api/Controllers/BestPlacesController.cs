using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.DataLayer.Models.Base;
using SizeUp.Core.DataLayer;
using SizeUp.Core.DataLayer.Base;


namespace SizeUp.Web.Areas.Api.Controllers
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

        public ActionResult Index(int itemCount, int industryId, string attribute, Granularity granularity, long? regionId, long? stateId)
        {
            BestPlacesFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.Get(context, industryId, attribute, regionId, stateId, filters, granularity).Take(itemCount).ToList();
                var cities = output.Select(i => i.Place.City.Id).ToList();
                var counties = Core.DataLayer.Base.Place.Get(context).Where(i => cities.Contains(i.CityId)).Select(i => new { Id = i.CityId, County = i.County.Name }).ToList();
                output.ForEach(i => i.Place.City.Counties = counties.Where(c => c.Id == i.Place.City.Id).Select(c => new Core.DataLayer.Models.County { Name = c.County }).ToList());
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Bands(int itemCount, int bands, int industryId, string attribute, Granularity granularity, long? regionId, long? stateId)
        {
            BestPlacesFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.BestPlaces.Bands(context, industryId, attribute,itemCount, bands, regionId, stateId, filters, granularity);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
