using System.Web.Mvc;
using System.Web.Routing;

namespace SizeUp.Web.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            /*********user***********/
            context.MapRoute(
                "UserAuthenticated",
                "Api/User/Authenticated",
                new { controller = "User", action = "Authenticated" },
                new string[] { "SizeUp.Web.Areas.Api.Controllers" }
            );


            /*********industry***********/
            context.MapRoute(
                "SingleIndustry",
                "Api/Industry/",
                new { controller = "Industry", action = "Industry" }
            );

            context.MapRoute(
               "IndustryList",
               "Api/Industry/List",
               new { controller = "Industry", action = "IndustryList" }
           );

            context.MapRoute(
                "SearchIndustry",
                "Api/Industry/Search",
                new { controller = "Industry", action = "SearchIndustries" }
            );

            context.MapRoute(
                "CurrentIndustry",
                "Api/Industry/Current",
                new { controller = "Industry", action = "CurrentIndustry" }
            );

            context.MapRoute(
                "HasData",
                "Api/Industry/HasData",
                new { controller = "Industry", action = "HasData" }
            );
            
            /***********place**********/

            context.MapRoute(
                "SearchPlace",
                "Api/Place/Search",
                new { controller = "Place", action = "SearchPlaces" }
            );

            context.MapRoute(
               "CurrentPlace",
               "Api/Place/Current",
               new { controller = "Place", action = "CurrentPlace" }
           );

            context.MapRoute(
               "DetectedPlace",
               "Api/Place/Detected",
               new { controller = "Place", action = "DetectedPlace" }
           );

            context.MapRoute(
               "GetPlace",
               "Api/Place/",
               new { controller = "Place", action = "Get" }
           );

            context.MapRoute(
             "CentroidPlace",
             "Api/Place/Centroid",
             new { controller = "Place", action = "Centroid" }
            );

            /***********city**********/
            context.MapRoute(
                "SingleCity",
                "Api/City/",
                new { controller = "City", action = "City" }
            );

            context.MapRoute(
              "BoundingBoxCity",
              "Api/City/BoundingBox",
              new { controller = "City", action = "BoundingBox" }
          );

            context.MapRoute(
             "CentroidCity",
             "Api/City/Centroid",
             new { controller = "City", action = "Centroid" }
         );

            /***********county**********/
            context.MapRoute(
                "SingleCounty",
                "Api/County/",
                new { controller = "County", action = "County" }
            );

            context.MapRoute(
              "BoundingBoxCounty",
              "Api/County/BoundingBox",
              new { controller = "County", action = "BoundingBox" }
          );

            context.MapRoute(
             "CentroidCounty",
             "Api/County/Centroid",
             new { controller = "County", action = "Centroid" }
         );

            

            /***********metro**********/
            context.MapRoute(
                "SingleMetro",
                "Api/Metro/",
                new { controller = "Metro", action = "Metro" }
            );

            context.MapRoute(
              "BoundingBoxMetro",
              "Api/Metro/BoundingBox",
              new { controller = "Metro", action = "BoundingBox" }
          );

            context.MapRoute(
             "CentroidMetro",
             "Api/Metro/Centroid",
             new { controller = "Metro", action = "Centroid" }
         );

            /***********state**********/
            context.MapRoute(
                "SingleState",
                "Api/State/",
                new { controller = "State", action = "State" }
            );

            context.MapRoute(
              "BoundingBoxState",
              "Api/State/BoundingBox",
              new { controller = "State", action = "BoundingBox" }
          );

            context.MapRoute(
             "CentroidState",
             "Api/State/Centroid",
             new { controller = "State", action = "Centroid" }
         );


            /***********business**********/
            context.MapRoute(
                "Api.Business",
                "Api/Business/",
                new { controller = "Business", action = "Business" },
                new string[] { "SizeUp.Web.Areas.Api.Controllers" }
            );

            context.MapRoute(
               "BusinessAt",
               "Api/Business/At",
               new { controller = "Business", action = "BusinessAt" },
               new string[] { "SizeUp.Web.Areas.Api.Controllers" }
           );


            context.MapRoute(
                "BusinessList",
                "Api/Business/List",
                new { controller = "Business", action = "BusinessList", page = UrlParameter.Optional, radius = UrlParameter.Optional },
                new string[] { "SizeUp.Web.Areas.Api.Controllers" }
            );


            /****AverageRevenue*****/

            context.MapRoute(
                "AverageRevenue",
                "Api/AverageRevenue/",
                new { controller = "AverageRevenue", action = "AverageRevenue" }
            );

            context.MapRoute(
                "AverageRevenuePercentle",
                "Api/AverageRevenue/Percentile",
                new { controller = "AverageRevenue", action = "Percentile" }
            );

            context.MapRoute(
                "AverageRevenueBands",
                "Api/AverageRevenue/Bands",
                new { controller = "AverageRevenue", action = "Bands" }
            );


            /*****year started *****/
            context.MapRoute(
               "YearStarted",
               "Api/YearStarted/",
               new { controller = "YearStarted", action = "YearStarted" }
           );

            context.MapRoute(
               "YearStartedPercentile",
               "Api/YearStarted/Percentile",
               new { controller = "YearStarted", action = "Percentile" }
           );

            /****AverageSalary*****/

            context.MapRoute(
                "AverageSalary",
                "Api/AverageSalary/",
                new { controller = "AverageSalary", action = "AverageSalary" }
            );

            context.MapRoute(
                "AverageSalaryPercentage",
                "Api/AverageSalary/Percentage",
                new { controller = "AverageSalary", action = "Percentage" }
            );

            context.MapRoute(
                "AverageSalaryBands",
                "Api/AverageSalary/Bands",
                new { controller = "AverageSalary", action = "Bands" }
            );


            /****AverageEmployees*****/

            context.MapRoute(
                "AverageEmployees",
                "Api/AverageEmployees/",
                new { controller = "AverageEmployees", action = "AverageEmployees" }
            );

            context.MapRoute(
                "AverageEmployeesPercentle",
                "Api/AverageEmployees/Percentile",
                new { controller = "AverageEmployees", action = "Percentile" }
            );

            context.MapRoute(
                "AverageEmployeesBands",
                "Api/AverageEmployees/Bands",
                new { controller = "AverageEmployees", action = "Bands" }
            );

            /*
            context.MapRoute(
                "AverageEmployeesBandsZip",
                "Api/AverageEmployees/Bands/Zip",
                new { controller = "AverageEmployees", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageEmployeesBandsCounty",
                "Api/AverageEmployees/Bands/County",
                new { controller = "AverageEmployees", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageEmployeesBandsState",
                "Api/AverageEmployees/Bands/State",
                new { controller = "AverageEmployees", action = "BandsByState" }
            );*/

            /****TotalEmployees*****/

            context.MapRoute(
                "TotalEmployees",
                "Api/TotalEmployees/",
                new { controller = "TotalEmployees", action = "TotalEmployees" }
            );

            context.MapRoute(
                "TotalEmployeesBands",
                "Api/TotalEmployees/Bands",
                new { controller = "TotalEmployees", action = "Bands"}
            );

            /****EmployeesPerCapita*****/

            context.MapRoute(
                "EmployeesPerCapita",
                "Api/EmployeesPerCapita/",
                new { controller = "EmployeesPerCapita", action = "EmployeesPerCapita" }
            );

            context.MapRoute(
                "EmployeesPerCapitaPercentle",
                "Api/EmployeesPerCapita/Percentile",
                new { controller = "EmployeesPerCapita", action = "Percentile" }
            );

            context.MapRoute(
                "EmployeesPerCapitaBands",
                "Api/EmployeesPerCapita/Bands",
                new { controller = "EmployeesPerCapita", action = "Bands" }
            );

           
           

            /****RevenuePerCapita*****/

            context.MapRoute(
                "RevenuePerCapita",
                "Api/RevenuePerCapita/",
                new { controller = "RevenuePerCapita", action = "RevenuePerCapita" }
            );

            context.MapRoute(
                "RevenuePerCapitaPercentile",
                "Api/RevenuePerCapita/Percentile",
                new { controller = "RevenuePerCapita", action = "Percentile" }
            );

            context.MapRoute(
                "RevenuePerCapitaBandsZip",
                "Api/RevenuePerCapita/Bands/Zip",
                new { controller = "RevenuePerCapita", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "RevenuePerCapitaBandsCounty",
                "Api/RevenuePerCapita/Bands/County",
                new { controller = "RevenuePerCapita", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "RevenuePerCapitaBandsState",
                "Api/RevenuePerCapita/Bands/State",
                new { controller = "RevenuePerCapita", action = "BandsByState" }
            );

            /****TotalRevenue*****/

            context.MapRoute(
               "TotalRevenue",
               "Api/TotalRevenue/",
               new { controller = "TotalRevenue", action = "TotalRevenue" }
           );

            context.MapRoute(
                "TotalRevenueBandsZip",
                "Api/TotalRevenue/Bands/Zip",
                new { controller = "TotalRevenue", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "TotalRevenueBandsCounty",
                "Api/TotalRevenue/Bands/County",
                new { controller = "TotalRevenue", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "TotalRevenueBandsState",
                "Api/TotalRevenue/Bands/State",
                new { controller = "TotalRevenue", action = "BandsByState" }
            );

            /****CostEffectiveness*****/

            context.MapRoute(
                "CostEffectiveness",
                "Api/CostEffectiveness/",
                new { controller = "CostEffectiveness", action = "CostEffectiveness" }
            );

            context.MapRoute(
                "CostEffectivenessPrecentage",
                "Api/CostEffectiveness/percentage",
                new { controller = "CostEffectiveness", action = "Percentage" }
            );

            context.MapRoute(
                "CostEffectivenessBandsCounty",
                "Api/CostEffectiveness/Bands/County",
                new { controller = "CostEffectiveness", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "CostEffectivenessBandsState",
                "Api/CostEffectiveness/Bands/State",
                new { controller = "CostEffectiveness", action = "BandsByState" }
            );


            /****turnover*****/

            context.MapRoute(
                "Turnover",
                "Api/Turnover/",
                new { controller = "Turnover", action = "Turnover" }
            );

            context.MapRoute(
                "TurnoverPercentile",
                "Api/Turnover/percentile",
                new { controller = "Turnover", action = "Percentile" }
            );

            /****jobchange*****/

            context.MapRoute(
                "JobChange",
                "Api/JobChange/",
                new { controller = "JobChange", action = "JobChange" }
            );

            /****healthcare*****/

            context.MapRoute(
                "HealthCare",
                "Api/HealthCare/",
                new { controller = "HealthCare", action = "HealthCare" }
            );
       
            context.MapRoute(
                "HealthCarePercentage",
                "Api/Turnover/percentage",
                new { controller = "HealthCare", action = "Percentage" }
            );

            /****workerscomp*****/

            context.MapRoute(
                "WorkersComp",
                "Api/WorkersComp/",
                new { controller = "WorkersComp", action = "WorkersComp" }
            );

            context.MapRoute(
               "WorkersCompPercentage",
               "Api/WorkersComp/Percentage",
               new { controller = "WorkersComp", action = "Percentage" }
           );



            /****advertising*****/

            context.MapRoute(
                "BestPlacesToAdvertise",
                "Api/Advertising/",
                new { controller = "Advertising", action = "Advertising" }
            );

            context.MapRoute(
                "BestPlacesToAdvertiseMinDistance",
                "Api/Advertising/MinimumDistance",
                new { controller = "Advertising", action = "MinimumDistance" }
            );

            context.MapRoute(
               "BestPlacesToAdvertiseBands",
               "Api/Advertising/Bands",
               new { controller = "Advertising", action = "Bands" }
           );


            /****demographics*****/

            context.MapRoute(
                "CityDemographics",
                "Api/Demographics/city",
                new { controller = "Demographics", action = "City" }
            );

            context.MapRoute(
                "CountyDemographics",
                "Api/Demographics/county",
                new { controller = "Demographics", action = "County" }
            );

            context.MapRoute(
                "MetroDemographics",
                "Api/Demographics/metro",
                new { controller = "Demographics", action = "Metro" }
            );

            context.MapRoute(
                "StateDemographics",
                "Api/Demographics/state",
                new { controller = "Demographics", action = "State" }
            );


              /****profile*****/

            context.MapRoute(
                "DashboardValuesGet",
                "Api/profile/dashboardValues",
                new { controller = "Profile", action = "GetDashboardValues" },
                 new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
               "DashboardValuesSet",
               "Api/profile/dashboardValues",
               new { controller = "Profile", action = "SetDashboardValues" },
                new { httpMethod = new HttpMethodConstraint("POST") }
           );

            context.MapRoute(
                "CompetitionValuesGet",
                "Api/profile/competitionValues",
                new { controller = "Profile", action = "GetCompetitionValues" },
                 new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
               "CompetitionValuesSet",
               "Api/profile/competitionValues",
               new { controller = "Profile", action = "SetCompetitionValues" },
                new { httpMethod = new HttpMethodConstraint("POST") }
           );



            /****consumerEpenditures*****/

            context.MapRoute(
                "ConsumerExpendituresBands",
                "Api/ConsumerExpenditures/Bands/{aggregationLevel}",
                new { controller = "ConsumerExpenditures", action = "Index", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "ConsumerExpendituresVariables",
                "Api/ConsumerExpenditures/Variables",
                new { controller = "ConsumerExpenditures", action = "Variables", parentId = UrlParameter.Optional }
            );

            context.MapRoute(
                "ConsumerExpendituresVariable",
                "Api/ConsumerExpenditures/Variable",
                new { controller = "ConsumerExpenditures", action = "Variable" }
            );

            context.MapRoute(
                "ConsumerExpendituresVariablePath",
                "Api/ConsumerExpenditures/VariablePath",
                new { controller = "ConsumerExpenditures", action = "VariablePath" }
            );

            context.MapRoute(
                "ConsumerExpendituresVariableCrosswalk",
                "Api/ConsumerExpenditures/VariableCrosswalk",
                new { controller = "ConsumerExpenditures", action = "VariableCrosswalk" }
            );



            context.MapRoute(
               "BestPlacesCity",
               "Api/BestPlaces/City",
               new { controller = "BestPlaces", action = "City" },
               new string[] { "SizeUp.Web.Areas.Api.Controllers" }
           );

            context.MapRoute(
              "BestPlacesCounty",
              "Api/BestPlaces/County",
              new { controller = "BestPlaces", action = "County" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesMetro",
              "Api/BestPlaces/Metro",
              new { controller = "BestPlaces", action = "Metro" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesState",
              "Api/BestPlaces/State",
              new { controller = "BestPlaces", action = "State" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );


            context.MapRoute(
               "BestPlacesBandsCity",
               "Api/BestPlaces/Bands/City",
               new { controller = "BestPlaces", action = "CityBands" },
               new string[] { "SizeUp.Web.Areas.Api.Controllers" }
           );

            context.MapRoute(
              "BestPlacesBandsCounty",
              "Api/BestPlaces/Bands/County",
              new { controller = "BestPlaces", action = "CountyBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesBandsMetro",
              "Api/BestPlaces/Bands/Metro",
              new { controller = "BestPlaces", action = "MetroBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesBandsState",
              "Api/BestPlaces/Bands/State",
              new { controller = "BestPlaces", action = "StateBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

           

            /*****************default*****************/
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
