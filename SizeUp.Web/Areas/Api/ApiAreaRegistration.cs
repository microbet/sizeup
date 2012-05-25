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
            /*********industry***********/
            context.MapRoute(
                "SingleIndustry",
                "Api/Industry/",
                new { controller = "Industry", action = "Industry" }
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
            

            /***********city**********/
            context.MapRoute(
                "SingleCity",
                "Api/City/",
                new { controller = "City", action = "City" }
            );

            context.MapRoute(
                "SearchCity",
                "Api/City/Search",
                new { controller = "City", action = "SearchCities" }
            );

            context.MapRoute(
               "CurrentCity",
               "Api/City/Current",
               new { controller = "City", action = "CurrentCity" }
           );

            context.MapRoute(
               "DetectedCity",
               "Api/City/Detected",
               new { controller = "City", action = "DetectedCity" }
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

         


            /****AverageSalary*****/

            context.MapRoute(
                "AverageSalary",
                "Api/AverageSalary/",
                new { controller = "AverageSalary", action = "AverageSalary", countyId = UrlParameter.Optional }
            );

            context.MapRoute(
                "AverageSalaryPercentage",
                "Api/AverageSalary/Percentage",
                new { controller = "AverageSalary", action = "Percentage", countyId = UrlParameter.Optional }
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

            /****turnover*****/

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
