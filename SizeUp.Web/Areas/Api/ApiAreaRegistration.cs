using System.Web.Mvc;

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

            

            /***********metro**********/
            context.MapRoute(
                "SingleMetro",
                "Api/Metro/",
                new { controller = "Metro", action = "Metro" }
            );

            /***********state**********/
            context.MapRoute(
                "SingleState",
                "Api/State/",
                new { controller = "State", action = "State" }
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
                "AverageRevenueBandsZip",
                "Api/AverageRevenue/Bands/Zip",
                new { controller = "AverageRevenue", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageRevenueBandsCounty",
                "Api/AverageRevenue/Bands/County",
                new { controller = "AverageRevenue", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageRevenueBandsState",
                "Api/AverageRevenue/Bands/State",
                new { controller = "AverageRevenue", action = "BandsByState" }
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

            context.MapRoute(
               "YearStartedCount",
               "Api/YearStarted/Count",
               new { controller = "YearStarted", action = "YearStartedCount" }
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
                "Api/AverageSalary/Bands/County",
                new { controller = "AverageSalary", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageSalaryState",
                "Api/AverageSalary/Bands/State",
                new { controller = "AverageSalary", action = "BandsByState" }
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
            );

            /****TotalEmployees*****/

            context.MapRoute(
                "TotalEmployees",
                "Api/TotalEmployees/",
                new { controller = "TotalEmployees", action = "TotalEmployees" }
            );

            context.MapRoute(
                "TotalEmployeesBandsZip",
                "Api/TotalEmployees/Bands/Zip",
                new { controller = "TotalEmployees", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "TotalEmployeesBandsCounty",
                "Api/TotalEmployees/Bands/County",
                new { controller = "TotalEmployees", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "TotalEmployeesBandsState",
                "Api/TotalEmployees/Bands/State",
                new { controller = "TotalEmployees", action = "BandsByState" }
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
                "EmployeesPerCapitaBandsZip",
                "Api/EmployeesPerCapita/Bands/Zip",
                new { controller = "EmployeesPerCapita", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeesPerCapitaBandsCounty",
                "Api/EmployeesPerCapita/Bands/County",
                new { controller = "EmployeesPerCapita", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "EmployeesPerCapitaBandsState",
                "Api/EmployeesPerCapita/Bands/State",
                new { controller = "EmployeesPerCapita", action = "BandsByState" }
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

  



            /*****************default*****************/
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
