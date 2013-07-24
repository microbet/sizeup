using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Demographics : ChartItem
    {
        public long? Id { get; set; }
        public long? Population { get; set; }
        public long? LaborForce { get; set; }
        public double? JobGrowth { get; set; }
        public double? Unemployment { get; set; }
        public double? MedianAge { get; set; }
        public double? AverageHouseholdExpenditures { get; set; }
        public long? HouseholdIncome { get; set; }
        public double? PersonalIncomeTax { get; set; }
        public double? PersonalCapitalGainsTax { get; set; }
        public double? CorporateIncomeTax { get; set; }
        public double? CorporateCapitalGainsTax { get; set; }
        public double? SalesTax { get; set; }
        public double? PropertyTax { get; set; }
        public long? HomeValue { get; set; }
        public double? BachelorsOrHigherPercentage { get; set; }
        public double? HighschoolOrHigherPercentage { get; set; }
        public double? WhiteCollarWorkersPercentage { get; set; }
        public double? BlueCollarWorkersPercentage { get; set; }
        public double? VeryCreativeProfessionalsPercentage { get; set; }
        public double? CreativeProfessionalsPercentage { get; set; }
        public double? YoungEducatedPercentage { get; set; }
        public double? InternationalTalentPercentage { get; set; }
        public int? Universities { get; set; }
        public int? Universities50Miles { get; set; }
        public double? CommuteTime { get; set; }
        public long? SmallBusinesses { get; set; }
    }
}
