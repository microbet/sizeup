using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class AdvertisingFilters
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

        public int? Distance { get; set; }

        public string Sort { get; set; }
        public string Attribute { get; set; }
        public string SortAttribute { get; set; }
        public string Order { get; set; }
    }
}
