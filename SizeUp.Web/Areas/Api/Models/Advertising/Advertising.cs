using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Advertising
{
    public class Advertising
    {

        public long ZipCodeId { get; set; }
        public City.City City { get; set; }
        public County.County County { get; set; }
        public State.State State { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public string Name { get; set; }
        public long? TotalRevenue { get; set; }
        public long? AverageRevenue { get; set; }
        public long? TotalEmployees { get; set; }
        public long? RevenuePerCapita { get; set; }
        public long? HouseholdIncome { get; set; }
        public double? HouseholdExpenditures { get; set; }
        public long? TotalPopulation { get; set; }
        public double? MedianAge { get; set; }
        public double? BachelorsDegreeOrHigher { get; set; }
        public double? HighSchoolOrHigher { get; set; }
        public double? WhiteCollarWorkers { get; set; }

    }
}