using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer.Projections
{
    public static class Demographics
    {
        public class Default : Projection<Data.Demographic, Models.Demographics>
        {
            public override Expression<Func<Data.Demographic, Models.Demographics>> Expression
            {
                get
                {
                    return i => new Models.Demographics()
                    {
                        Id = i.GeographicLocationId,
                        Name = i.GeographicLocation.LongName,
                        AverageHouseholdExpenditures = i.AverageHouseholdExpenditure,
                        BachelorsOrHigherPercentage = i.BachelorsOrHigherPercentage * 100,
                        BlueCollarWorkersPercentage = i.BlueCollarWorkersPercentage * 100,
                        CommuteTime = i.CommuteTime,
                        CorporateCapitalGainsTax = i.CorporateCapitalGainsTax,
                        CorporateIncomeTax = i.CorporateIncomeTax,
                        CreativeProfessionalsPercentage = i.CreativePercentage * 100,
                        HighschoolOrHigherPercentage = i.HighSchoolOrHigherPercentage * 100,
                        HomeValue = i.HomeValue,
                        HouseholdIncome = i.MedianHouseholdIncome,
                        InternationalTalentPercentage = i.InternationalTalent,
                        JobGrowth = i.JobGrowth * 100,
                        LaborForce = i.LaborForce,
                        MedianAge = i.MedianAge,
                        PersonalCapitalGainsTax = i.PersonalCapitalGainsTax,
                        PersonalIncomeTax = i.PersonalIncomeTax,
                        Population = i.TotalPopulation,
                        PropertyTax = i.PropertyTax * 100,
                        SalesTax = i.SalesTax,
                        SmallBusinesses = i.TotalEstablishments,
                        Universities = i.UniversitiesWithinHalfMile,
                        Universities50Miles = i.UniversitiesWithin50Miles,
                        VeryCreativeProfessionalsPercentage = i.VeryCreativePercentage * 100,
                        WhiteCollarWorkersPercentage = i.WhiteCollarWorkersPercentage * 100,
                        YoungEducatedPercentage = i.YoungEducatedPercentage * 100,
                        Unemployment = i.Unemployment
                    };
                }
            }
        }
    }
}
