using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Advertising
    {
        public Models.Place Place { get; set; }
        public Models.ZipCode ZipCode { get; set; }
        public Core.Geo.LatLng Centroid { get; set; }
        public long? AverageRevenue { get; set; }
        public long? TotalRevenue { get; set; }
        public long? TotalEmployees { get; set; }
        public long? RevenuePerCapita { get; set; }
        public long? HouseholdIncome { get; set; }
        public long? Population { get; set; }
        public double? BachelorsDegreeOrHigher { get; set; }
        public double? HighSchoolOrHigher { get; set; }
        public double? WhiteCollarWorkers { get; set; }
        public double? MedianAge { get; set; }
        public double? HouseholdExpenditures { get; set; }
    }
}
