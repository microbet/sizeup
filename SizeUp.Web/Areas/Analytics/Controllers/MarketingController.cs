using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core;
using SizeUp.Core.Analytics;
namespace Sizeup.Web.Areas.Analytics.Controllers
{
    public class MarketingController : Controller
    {
        [HttpPost]
        public bool ReportLoaded(long industryId, long geographicLocationId)
        {
            Guid? userId = null;

            if (User.Identity.IsAuthenticated)
                userId = (Guid)Membership.GetUser().ProviderUserKey;
            
            Singleton<Tracker>.Instance.AdvertisingReportLoaded(new SizeUp.Data.Analytics.AdvertisingAttribute()
            {
                IndustryId = industryId,
                UserId = userId,
                GeographicLocationId = geographicLocationId,
                Distance = Request.Form["distance"] == null ? null : (long?)long.Parse(Request.Form["distance"]),
                AverageRevenueMin = ParseMin(Request.Form["averageRevenue"]),
                AverageRevenueMax = ParseMax(Request.Form["averageRevenue"]),
                TotalRevenueMin = ParseMin(Request.Form["totalRevenue"]),
                TotalRevenueMax = ParseMax(Request.Form["totalRevenue"]),
                RevenuePerCapitaMin = ParseMin(Request.Form["revenuePerCapita"]),
                RevenuePerCapitaMax = ParseMax(Request.Form["revenuePerCapita"]),
                TotalEmployeesMin = ParseMin(Request.Form["totalEmployees"]),
                TotalEmployeesMax = ParseMax(Request.Form["totalEmployees"]),
                TotalPeopleMin = ParseMin(Request.Form["totalPeople"]),
                TotalPeopleMax = ParseMax(Request.Form["totalPeople"]),
                PercentForeignMin = (int?)ParseMin(Request.Form["percentForeign"]),
                PercentForeignMax = (int?)ParseMax(Request.Form["percentForeign"]),
                PercentMaleMin = (int?)ParseMin(Request.Form["percentMale"]),
                PercentMaleMax = (int?)ParseMax(Request.Form["percentMale"]),
                PercentFemaleMin = (int?)ParseMin(Request.Form["percentFemale"]),
                PercentFemaleMax = (int?)ParseMax(Request.Form["percentFemale"]),
                PercentAgeBelow5Min = (int?)ParseMin(Request.Form["percentAgeBelow5"]),
                PercentAgeBelow5Max = (int?)ParseMax(Request.Form["percentAgeBelow5"]),
                PercentAge5To19Min = (int?)ParseMin(Request.Form["percentAge5To19"]),
                PercentAge5To19Max = (int?)ParseMax(Request.Form["percentAge5To19"]),
                PercentAge20To29Min = (int?)ParseMin(Request.Form["percentAge20To29"]),
                PercentAge20To29Max = (int?)ParseMax(Request.Form["percentAge20To29"]),
                PercentAge30To39Min = (int?)ParseMin(Request.Form["percentAge30To39"]),
                PercentAge30To39Max = (int?)ParseMax(Request.Form["percentAge30To39"]),
                PercentAge40To49Min = (int?)ParseMin(Request.Form["percentAge40To49"]),
                PercentAge40To49Max = (int?)ParseMax(Request.Form["percentAge40To49"]),
                PercentAge50To64Min = (int?)ParseMin(Request.Form["percentAge50To64"]),
                PercentAge50To64Max = (int?)ParseMax(Request.Form["percentAge50To64"]),
                PercentAgeAbove65Min = (int?)ParseMin(Request.Form["percentAgeAbove65"]),
                PercentAgeAbove65Max = (int?)ParseMax(Request.Form["percentAgeAbove65"])
            });

            return true;
        }
        private long? ParseMin(string toParse)
        {
            long parsed;
            long? nullableParsed = null;

            if(toParse != null && long.TryParse(toParse.Split(',')[0], out parsed)){
                nullableParsed = (long?)parsed;
            }
            return nullableParsed;
        }

        private long? ParseMax(string toParse)
        {
            long parsed;
            long? nullableParsed = null;

            if (toParse != null && long.TryParse(toParse.Split(',')[1], out parsed))
            {
                nullableParsed = (long?)parsed;
            }
            return nullableParsed;
        }

    }
}