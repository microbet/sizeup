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
                "Business",
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


            /****revenue*****/

            context.MapRoute(
                "Revenue",
                "Api/Revenue/",
                new { controller = "Revenue", action = "Revenue" }
            );

            context.MapRoute(
                "RevenuePercentle",
                "Api/Revenue/Percentile",
                new { controller = "Revenue", action = "Percentile" }
            );

            context.MapRoute(
                "RevenueBandsZip",
                "Api/Revenue/Bands/Zip",
                new { controller = "Revenue", action = "BandsByZip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "RevenueBandsCounty",
                "Api/Revenue/Bands/County",
                new { controller = "Revenue", action = "BandsByCounty", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
                "RevenueBandsState",
                "Api/Revenue/Bands/State",
                new { controller = "Revenue", action = "BandsByState" }
            );

            /*****year started *****/
            context.MapRoute(
               "YearStarted",
               "Api/YearStarted/",
               new { controller = "YearStarted", action = "YearStarted" }
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

  



            /*****************default*****************/
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
