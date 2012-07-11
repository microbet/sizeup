using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Advertising
{
    public class Advertising
    {
        public long ZipCodeId { get; set; }
        public string Name { get; set; }
        public long TotalRevenue { get; set; }
        public long AverageRevenue { get; set; }
        public long TotalEmployees { get; set; }
        public long RevenuePerCapita { get; set; }
        public long HouseholdIncome { get; set; }
        public long HouseholdExpendatures { get; set; }
        public int MedianAge { get; set; }
        public int BachelorsDegreeOrHigher { get; set; }
        public int HighSchoolOrHigher { get; set; }
        public long WhiteCollarWorkers { get; set; }

    }
}