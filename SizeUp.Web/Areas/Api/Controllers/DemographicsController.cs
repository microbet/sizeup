using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Controllers
{
    public class DemographicsController : BaseController
    {
        //
        // GET: /Api/Demographics/

        public ActionResult City(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
   
                var entities = context.Cities.Select(i => new
                {
                    City = i,
                    CityDemographics = i.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault(),
                    StateDemographics = i.State.DemographicsByStates.Where(d=> d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                var data = entities.Where(i => i.City.Id == id)
                    .Select(i => new Models.Demographics.Place()
                    {
                        Id = id,
                        AverageHouseholdExpenditures = i.CityDemographics.AverageHouseholdExpenditure,
                        BachelorsOrHigherPercentage = i.CityDemographics.BachelorsOrHigherPercentage * 100,
                        BlueCollarWorkersPercentage = i.CityDemographics.BlueCollarWorkersPercentage * 100,
                        CommuteTime = i.CityDemographics.CommuteTime,
                        CorporateCapitalGainsTax = i.StateDemographics.CorporateCapitalGainsTax,
                        CorporateIncomeTax = i.StateDemographics.CorporateIncomeTax,
                        CreativeProfessionalsPercentage = i.CityDemographics.CreativePercentage * 100,
                        HighschoolOrHigherPercentage = i.CityDemographics.HighSchoolOrHigherPercentage * 100,
                        HomeValue = i.CityDemographics.HomeValue,
                        HouseholdIncome = i.CityDemographics.MedianHouseholdIncome,
                        InternationalTalentPercentage = i.CityDemographics.InternationalTalent,
                        JobGrowth = i.CityDemographics.JobGrowth * 100,
                        LaborForce = i.CityDemographics.LaborForce,
                        MedianAge = i.CityDemographics.MedianAge,
                        PersonalCapitalGainsTax = i.StateDemographics.PersonalCapitalGainsTax,
                        PersonalIncomeTax = i.StateDemographics.PersonalIncomeTax,
                        Population = i.CityDemographics.TotalPopulation,
                        PropertyTax = i.CityDemographics.PropertyTax * 100,
                        SalesTax = i.StateDemographics.SalesTax,
                        SmallBusinesses = i.CityDemographics.TotalEstablishments,
                        Unemployment = i.CityDemographics.Unemployment,
                        Universities = i.CityDemographics.UniversitiesWithinHalfMile,
                        Universities50Miles = i.CityDemographics.UniversitiesWithin50Miles,
                        VeryCreativeProfessionalsPercentage = i.CityDemographics.VeryCreativePercentage * 100,
                        WhiteCollarWorkersPercentage = i.CityDemographics.WhiteCollarWorkersPercentage * 100,
                        YoungEducatedPercentage = i.CityDemographics.YoungEducatedPercentage * 100
                    })
                    .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult County(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.Counties.Select(i => new
                {
                    County = i,
                    CountyDemographics = i.DemographicsByCounties.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault(),
                    StateDemographics = i.State.DemographicsByStates.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                var data = entities.Where(i => i.County.Id == id)
                    .Select(i => new Models.Demographics.Place()
                    {
                        Id = id,
                        AverageHouseholdExpenditures = i.CountyDemographics.AverageHouseholdExpenditure,
                        BachelorsOrHigherPercentage = i.CountyDemographics.BachelorsOrHigherPercentage * 100,
                        BlueCollarWorkersPercentage = i.CountyDemographics.BlueCollarWorkersPercentage * 100,
                        CommuteTime = i.CountyDemographics.CommuteTime,
                        CorporateCapitalGainsTax = i.StateDemographics.CorporateCapitalGainsTax,
                        CorporateIncomeTax = i.StateDemographics.CorporateIncomeTax,
                        CreativeProfessionalsPercentage = i.CountyDemographics.CreativePercentage * 100,
                        HighschoolOrHigherPercentage = i.CountyDemographics.HighSchoolOrHigherPercentage * 100,
                        HomeValue = i.CountyDemographics.HomeValue,
                        HouseholdIncome = i.CountyDemographics.MedianHouseholdIncome,
                        InternationalTalentPercentage = i.CountyDemographics.InternationalTalent,
                        JobGrowth = i.CountyDemographics.JobGrowth * 100,
                        LaborForce = i.CountyDemographics.LaborForce,
                        MedianAge = i.CountyDemographics.MedianAge,
                        PersonalCapitalGainsTax = i.StateDemographics.PersonalCapitalGainsTax,
                        PersonalIncomeTax = i.StateDemographics.PersonalIncomeTax,
                        Population = i.CountyDemographics.TotalPopulation,
                        PropertyTax = i.CountyDemographics.PropertyTax * 100,
                        SalesTax = i.StateDemographics.SalesTax,
                        SmallBusinesses = i.CountyDemographics.TotalEstablishments,
                        Universities = i.CountyDemographics.UniversitiesWithinHalfMile,
                        Universities50Miles = i.CountyDemographics.UniversitiesWithin50Miles,
                        VeryCreativeProfessionalsPercentage = i.CountyDemographics.VeryCreativePercentage * 100,
                        WhiteCollarWorkersPercentage = i.CountyDemographics.WhiteCollarWorkersPercentage * 100,
                        YoungEducatedPercentage = i.CountyDemographics.YoungEducatedPercentage * 100
                    })
                    .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Metro(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
    
                var entities = context.Metroes.Select(i => new
                {
                    Metro = i,
                    CountyDemographics = i.DemographicsByMetroes.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                var data = entities.Where(i => i.Metro.Id == id)
                    .Select(i => new Models.Demographics.Place()
                    {
                        Id = id,
                        AverageHouseholdExpenditures = i.CountyDemographics.AverageHouseholdExpenditure,
                        BachelorsOrHigherPercentage = i.CountyDemographics.BachelorsOrHigherPercentage * 100,
                        BlueCollarWorkersPercentage = i.CountyDemographics.BlueCollarWorkersPercentage * 100,
                        CommuteTime = i.CountyDemographics.CommuteTime,
  
                        CreativeProfessionalsPercentage = i.CountyDemographics.CreativePercentage * 100,
                        HighschoolOrHigherPercentage = i.CountyDemographics.HighSchoolOrHigherPercentage * 100,
                        HomeValue = i.CountyDemographics.HomeValue,
                        HouseholdIncome = i.CountyDemographics.MedianHouseholdIncome,
                        InternationalTalentPercentage = i.CountyDemographics.InternationalTalent,
                        JobGrowth = i.CountyDemographics.JobGrowth * 100,
                        LaborForce = i.CountyDemographics.LaborForce,
                        MedianAge = i.CountyDemographics.MedianAge,
    
                        Population = i.CountyDemographics.TotalPopulation,
                        PropertyTax = i.CountyDemographics.PropertyTax * 100,

                        SmallBusinesses = i.CountyDemographics.TotalEstablishments,
                        Universities = i.CountyDemographics.UniversitiesWithinHalfMile,
                        Universities50Miles = i.CountyDemographics.UniversitiesWithin50Miles,
                        VeryCreativeProfessionalsPercentage = i.CountyDemographics.VeryCreativePercentage * 100,
                        WhiteCollarWorkersPercentage = i.CountyDemographics.WhiteCollarWorkersPercentage * 100,
                        YoungEducatedPercentage = i.CountyDemographics.YoungEducatedPercentage * 100
                    })
                    .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult State(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.States.Select(i => new
                {
                    State = i,
                    StateDemographics = i.DemographicsByStates.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                var data = entities.Where(i => i.State.Id == id)
                    .Select(i => new Models.Demographics.Place()
                    {
                        Id = id,
                        AverageHouseholdExpenditures = i.StateDemographics.AverageHouseholdExpenditure,
                        BachelorsOrHigherPercentage = i.StateDemographics.BachelorsOrHigherPercentage * 100,
                        BlueCollarWorkersPercentage = i.StateDemographics.BlueCollarWorkersPercentage * 100,
                        CommuteTime = i.StateDemographics.CommuteTime,
                        CorporateCapitalGainsTax = i.StateDemographics.CorporateCapitalGainsTax,
                        CorporateIncomeTax = i.StateDemographics.CorporateIncomeTax,
                        CreativeProfessionalsPercentage = i.StateDemographics.CreativePercentage * 100,
                        HighschoolOrHigherPercentage = i.StateDemographics.HighSchoolOrHigherPercentage * 100,
                        HomeValue = i.StateDemographics.HomeValue,
                        HouseholdIncome = i.StateDemographics.MedianHouseholdIncome,
                        InternationalTalentPercentage = i.StateDemographics.InternationalTalent,
                        JobGrowth = i.StateDemographics.JobGrowth * 100,
                        LaborForce = i.StateDemographics.LaborForce,
                        MedianAge = i.StateDemographics.MedianAge,
                        PersonalCapitalGainsTax = i.StateDemographics.PersonalCapitalGainsTax,
                        PersonalIncomeTax = i.StateDemographics.PersonalIncomeTax,
                        Population = i.StateDemographics.TotalPopulation,
                        PropertyTax = i.StateDemographics.PropertyTax * 100,
                        SalesTax = i.StateDemographics.SalesTax,
                        SmallBusinesses = i.StateDemographics.TotalEstablishments,
                        Universities = i.StateDemographics.UniversitiesWithinHalfMile,
                        Universities50Miles = i.StateDemographics.UniversitiesWithin50Miles,
                        VeryCreativeProfessionalsPercentage = i.StateDemographics.VeryCreativePercentage * 100,
                        WhiteCollarWorkersPercentage = i.StateDemographics.WhiteCollarWorkersPercentage * 100,
                        YoungEducatedPercentage = i.StateDemographics.YoungEducatedPercentage * 100
                    })
                    .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
