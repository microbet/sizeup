using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class Demographics : Base.Base
    {
        public static Models.Demographics Get(SizeUpContext context, long id, Granularity granularity)
        {
            Models.Demographics output = null;
            var data = Get(context, granularity)
                .Where(i => i.Id == id);
            return output;
        }



        public static IQueryable<Models.Demographics> Get(SizeUpContext context,  Granularity granularity)
        {
            IQueryable<Models.Demographics> output = new List<Models.Demographics>().AsQueryable();//empty

            var zipData = DemographicsData.ZipCode(context);
            var cityData = DemographicsData.City(context);
            var countyData = DemographicsData.County(context);
            var metroData = DemographicsData.Metro(context);
            var stateData = DemographicsData.State(context);



            var zip = zipData.Select(i => new Models.Demographics
            {
                Id = i.ZipCodeId,
                AverageHouseholdExpenditures = i.AverageHouseholdExpenditure,
                BachelorsOrHigherPercentage = i.BachelorsOrHigherPercentage * 100,
                BlueCollarWorkersPercentage = i.BlueCollarWorkersPercentage * 100,
                HighschoolOrHigherPercentage = i.HighSchoolOrHigherPercentage * 100,
                HouseholdIncome = i.MedianHouseholdIncome,
                LaborForce = i.LaborForce,
                MedianAge = i.MedianAge,
                SmallBusinesses = i.TotalEstablishments,
                WhiteCollarWorkersPercentage = i.WhiteCollarWorkersPercentage * 100,
                Name = i.ZipCode.Name,
                Population = i.TotalPopulation
            });


            var city = cityData.Join(stateData, i => i.City.StateId, o => o.StateId, (c, s) => new { City = c, State = s })
                .Select(i => new Models.Demographics
                {
                    Id = i.City.CityId,
                    AverageHouseholdExpenditures = i.City.AverageHouseholdExpenditure,
                    BachelorsOrHigherPercentage = i.City.BachelorsOrHigherPercentage * 100,
                    BlueCollarWorkersPercentage = i.City.BlueCollarWorkersPercentage * 100,
                    CommuteTime = i.City.CommuteTime,
                    CorporateCapitalGainsTax = i.State.CorporateCapitalGainsTax,
                    CorporateIncomeTax = i.State.CorporateIncomeTax,
                    CreativeProfessionalsPercentage = i.City.CreativePercentage * 100,
                    HighschoolOrHigherPercentage = i.City.HighSchoolOrHigherPercentage * 100,
                    HomeValue = i.City.HomeValue,
                    HouseholdIncome = i.City.MedianHouseholdIncome,
                    InternationalTalentPercentage = i.City.InternationalTalent,
                    JobGrowth = i.City.JobGrowth * 100,
                    LaborForce = i.City.LaborForce,
                    MedianAge = i.City.MedianAge,
                    PersonalCapitalGainsTax = i.State.PersonalCapitalGainsTax,
                    PersonalIncomeTax = i.State.PersonalIncomeTax,
                    Population = i.City.TotalPopulation,
                    PropertyTax = i.City.PropertyTax * 100,
                    SalesTax = i.State.SalesTax,
                    SmallBusinesses = i.City.TotalEstablishments,
                    Unemployment = i.City.Unemployment,
                    Universities = i.City.UniversitiesWithinHalfMile,
                    Universities50Miles = i.City.UniversitiesWithin50Miles,
                    VeryCreativeProfessionalsPercentage = i.City.VeryCreativePercentage * 100,
                    WhiteCollarWorkersPercentage = i.City.WhiteCollarWorkersPercentage * 100,
                    YoungEducatedPercentage = i.City.YoungEducatedPercentage * 100,
                    Name = i.City.City.Name + ", " + i.State.State.Abbreviation
                });

            var county = countyData.Join(stateData, i => i.County.StateId, o => o.StateId, (c, s) => new { County = c, State = s })
                .Select(i => new Models.Demographics
                {
                    Id = i.County.CountyId,
                    AverageHouseholdExpenditures = i.County.AverageHouseholdExpenditure,
                    BachelorsOrHigherPercentage = i.County.BachelorsOrHigherPercentage * 100,
                    BlueCollarWorkersPercentage = i.County.BlueCollarWorkersPercentage * 100,
                    CommuteTime = i.County.CommuteTime,
                    CorporateCapitalGainsTax = i.State.CorporateCapitalGainsTax,
                    CorporateIncomeTax = i.State.CorporateIncomeTax,
                    CreativeProfessionalsPercentage = i.County.CreativePercentage * 100,
                    HighschoolOrHigherPercentage = i.County.HighSchoolOrHigherPercentage * 100,
                    HomeValue = i.County.HomeValue,
                    HouseholdIncome = i.County.MedianHouseholdIncome,
                    InternationalTalentPercentage = i.County.InternationalTalent,
                    JobGrowth = i.County.JobGrowth * 100,
                    LaborForce = i.County.LaborForce,
                    MedianAge = i.County.MedianAge,
                    PersonalCapitalGainsTax = i.State.PersonalCapitalGainsTax,
                    PersonalIncomeTax = i.State.PersonalIncomeTax,
                    Population = i.County.TotalPopulation,
                    PropertyTax = i.County.PropertyTax * 100,
                    SalesTax = i.State.SalesTax,
                    SmallBusinesses = i.County.TotalEstablishments,
                    Universities = i.County.UniversitiesWithinHalfMile,
                    Universities50Miles = i.County.UniversitiesWithin50Miles,
                    VeryCreativeProfessionalsPercentage = i.County.VeryCreativePercentage * 100,
                    WhiteCollarWorkersPercentage = i.County.WhiteCollarWorkersPercentage * 100,
                    YoungEducatedPercentage = i.County.YoungEducatedPercentage * 100,
                    Name = i.County.County.Name + ", " + i.State.State.Abbreviation
                });

            var metro = metroData
                .Select(i => new Models.Demographics
                {
                    Id = i.MetroId,
                    AverageHouseholdExpenditures = i.AverageHouseholdExpenditure,
                    BachelorsOrHigherPercentage = i.BachelorsOrHigherPercentage * 100,
                    BlueCollarWorkersPercentage = i.BlueCollarWorkersPercentage * 100,
                    CommuteTime = i.CommuteTime,
                    CreativeProfessionalsPercentage = i.CreativePercentage * 100,
                    HighschoolOrHigherPercentage = i.HighSchoolOrHigherPercentage * 100,
                    HomeValue = i.HomeValue,
                    HouseholdIncome = i.MedianHouseholdIncome,
                    InternationalTalentPercentage = i.InternationalTalent,
                    JobGrowth = i.JobGrowth * 100,
                    LaborForce = i.LaborForce,
                    MedianAge = i.MedianAge,
                    Population = i.TotalPopulation,
                    PropertyTax = i.PropertyTax * 100,
                    SmallBusinesses = i.TotalEstablishments,
                    Universities = i.UniversitiesWithinHalfMile,
                    Universities50Miles = i.UniversitiesWithin50Miles,
                    VeryCreativeProfessionalsPercentage = i.VeryCreativePercentage * 100,
                    WhiteCollarWorkersPercentage = i.WhiteCollarWorkersPercentage * 100,
                    YoungEducatedPercentage = i.YoungEducatedPercentage * 100,
                    Name = i.Metro.Name
                });


            var state = stateData
                .Select(i => new Models.Demographics
                {
                    Id = i.StateId,
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
                    Name = i.State.Name
                });


            if (granularity == Granularity.ZipCode)
            {
                output = zip;
            }
            else if (granularity == Granularity.City)
            {
                output = city;
            }
            else if (granularity == Granularity.County)
            {
                output = county;
            }
            else if (granularity == Granularity.Metro)
            {
                output = metro;
            }
            else if (granularity == Granularity.State)
            {
                output = state;
            }

            return output;
        }

  

    }
}
