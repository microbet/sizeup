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

        public ActionResult Demographics(long id)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var locations = Locations.Get(context, id).FirstOrDefault();
  
                var data = DemographicsData.GetCity(context, locations.City.Id)
                    .Join(DemographicsData.GetStates(context), i => i.City.StateId, o => o.StateId, (i, o) => new { CityData = i, StateData = o })
                    .Select(i => new Models.Demographics.Place()
                {
                    PlaceId = locations.Id,
                    AverageHouseholdExpenditures = i.CityData.AverageHouseholdExpenditure,
                    BachelorsOrHigherPercentage = i.CityData.BachelorsOrHigherPercentage * 100,
                    BlueCollarWorkersPercentage = i.CityData.BlueCollarWorkersPercentage * 100,
                    CommuteTime = i.CityData.AverageCommuteTime,
                    CorporateCapitalGainsTax = i.StateData.CorporateCapitalGainsTax,
                    CorporateIncomeTax = i.StateData.CorporateIncomeTax,
                    CreativeProfessionalsPercentage = i.CityData.CreativePercentage * 100,
                    HighschoolOrHigherPercentage = i.CityData.HighSchoolOrHigherPercentage * 100,
                    HomeValue = i.CityData.HomeValue,
                    HouseholdIncome = i.CityData.MedianHouseholdIncome,
                    InternationalTalentPercentage = i.CityData.InternationalTalent,
                    JobGrowth = i.CityData.JobGrowth * 100,
                    LaborForce = i.CityData.LaborForce,
                    MedianAge = i.CityData.MedianAge,
                    PersonalCapitalGainsTax = i.StateData.PersonalCapitalGainsTax,
                    PersonalIncomeTax = i.StateData.PersonalIncomeTax,
                    Population = i.CityData.TotalPopulation,
                    PropertyTax = i.CityData.PropertyTax * 100,
                    SalesTax = i.StateData.SalesTax,
                    SmallBusinesses = i.CityData.TotalEstablishments,
                    Unemployment = i.CityData.Unemployment,
                    Universities = i.CityData.UniversitiesWithinHalfMile,
                    Universities50Miles = i.CityData.UniversitiesWithin50Miles,
                    VeryCreativeProfessionalsPercentage = i.CityData.VeryCreativePercentage * 100,
                    WhiteCollarWorkersPercentage = i.CityData.WhiteCollarWorkersPercentage * 100,
                    YoungEducatedPercentage = i.CityData.YoungEducatedPercentage * 100

                })
                .FirstOrDefault();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
