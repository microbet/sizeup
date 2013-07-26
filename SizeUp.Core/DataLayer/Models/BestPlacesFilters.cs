using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class BestPlacesFilters
    {
        public Band<int?> AverageRevenue { get; set; }
        public Band<int?> TotalRevenue { get; set; }
        public Band<int?> AverageEmployees { get; set; }
        public Band<int?> TotalEmployees { get; set; }
        public Band<int?> RevenuePerCapita { get; set; }

        public Band<int?> HouseholdIncome { get; set; }
        public Band<int?> HouseholdExpenditures { get; set; }
        public Band<int?> MedianAge { get; set; }

        public int? BachelorOrHigher { get; set; }
        public int? HighSchoolOrHigher { get; set; }
        public int? WhiteCollarWorkers { get; set; }
        public int? BlueCollarWorkers { get; set; }
        public int? AirportsNearby { get; set; }
        public int? YoungEducated { get; set; }
        public int? UniversitiesNearby { get; set; }
        public int? CommuteTime { get; set; }


        public string Attribute { get; set; }
    }
}
