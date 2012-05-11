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


            /****state*****/

            context.MapRoute(
                "State",
                "Api/State/Polygon",
                new { controller = "State", action = "Polygon" }
            );

            context.MapRoute(
                "StateBounds",
                "Api/State/Contained",
                new { controller = "State", action = "Contained" }
            );

            /****county*****/

            context.MapRoute(
                "County",
                "Api/County/Polygon",
                new { controller = "County", action = "Polygon" }
            );

            context.MapRoute(
                "CountyBounds",
                "Api/County/Contained",
                new { controller = "County", action = "Contained" }
            );

            /****Zip*****/

            context.MapRoute(
                "Zip",
                "Api/Zip/Polygon",
                new { controller = "Zip", action = "Polygon" }
            );

            context.MapRoute(
                "ZipBounds",
                "Api/Zip/Contained",
                new { controller = "Zip", action = "Contained" }
            );


            /****salary*****/

            context.MapRoute(
                "Salary",
                "Api/Salary/",
                new { controller = "AverageSalary", action = "Salary", countyId = UrlParameter.Optional }
            );

            context.MapRoute(
                "SalaryPercentile",
                "Api/Salary/Percentile",
                new { controller = "AverageSalary", action = "Percentile", countyId = UrlParameter.Optional }
            );

            context.MapRoute(
                "SalaryBands",
                "Api/Salary/Bands/County",
                new { controller = "AverageSalary", action = "BandsByCounty" }
            );

            context.MapRoute(
                "SalaryMetro",
                "Api/Salary/Bands/Metro",
                new { controller = "AverageSalary", action = "BandsByMetro" }
            );

            context.MapRoute(
                "SalaryState",
                "Api/Salary/Bands/State",
                new { controller = "AverageSalary", action = "BandsByState" }
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
