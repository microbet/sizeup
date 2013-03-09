using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;
using System.Data.Spatial;
using Microsoft.SqlServer.Types;
using SizeUp.Web;
using SizeUp.Core;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer.Models;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class AdvertisingController : BaseController
    {
        //
        // GET: /Api/Advertising/
        private AdvertisingFilters BuildFilters()
        {
            AdvertisingFilters f = new AdvertisingFilters();
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
            f.Sort = QueryString.StringValue("sort");
            f.SortAttribute = QueryString.StringValue("sortAttribute");
            f.Attribute = QueryString.StringValue("attribute");
            f.Distance = QueryString.IntValue("distance");
            f.Order = QueryString.StringValue("order");
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


        public ActionResult Advertising(int industryId, long placeId, int page = 1, int itemCount = 20)
        {
            if (!User.Identity.IsAuthenticated)
            {
                page = 1;
                itemCount = 3;
            }

            AdvertisingFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Advertising.Get(context, industryId, placeId, filters);
                var output = new
                {
                    Total = data.Count(),
                    Items = data
                        .Skip((page - 1) * itemCount)
                        .Take(itemCount)
                        .ToList()
                };

                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult MinimumDistance(int industryId, long placeId, int itemCount)
        {
            AdvertisingFilters filters = BuildFilters();
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = Core.DataLayer.Advertising.MinimumDistance(context, industryId, placeId, itemCount, filters);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Bands(int industryId, long placeId, int bands)
        {
            AdvertisingFilters filters = BuildFilters();

            using (var context = ContextFactory.SizeUpContext)
            {
                var output = Core.DataLayer.Advertising.Bands(context, industryId, placeId, bands, filters);
                return Json(output, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
