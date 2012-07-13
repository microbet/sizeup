using System.Web.Mvc;

namespace SizeUp.Web.Areas.Tiles
{
    public class TilesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tiles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "AverageRevenueTilesZip",
                "Tiles/AverageRevenue/Zip/",
                new { controller = "AverageRevenue", action = "Zip", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
               "AverageRevenueTilesCounty",
               "Tiles/AverageRevenue/County/",
               new { controller = "AverageRevenue", action = "County", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "AverageRevenueTilesState",
               "Tiles/AverageRevenue/State/",
               new { controller = "AverageRevenue", action = "State", boundingEntityId = UrlParameter.Optional }
           );

     

            context.MapRoute(
                "AverageSalaryTilesCounty",
                "Tiles/AverageSalary/County/",
                new { controller = "AverageSalary", action = "County", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
               "AverageSalaryTilesState",
               "Tiles/AverageSalary/State/",
               new { controller = "AverageSalary", action = "State", boundingEntityId = UrlParameter.Optional }
           );


            context.MapRoute(
               "RevenuePerCapitaTilesZip",
               "Tiles/RevenuePerCapita/Zip/",
               new { controller = "RevenuePerCapita", action = "Zip", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "RevenuePerCapitaTilesCounty",
               "Tiles/RevenuePerCapita/County/",
               new { controller = "RevenuePerCapita", action = "County", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "RevenuePerCapitaTilesState",
               "Tiles/RevenuePerCapita/State/",
               new { controller = "RevenuePerCapita", action = "State", boundingEntityId = UrlParameter.Optional }
           );


            context.MapRoute(
               "TotalRevenueTilesZip",
               "Tiles/TotalRevenue/Zip/",
               new { controller = "TotalRevenue", action = "Zip", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "TotalRevenueTilesCounty",
               "Tiles/TotalRevenue/County/",
               new { controller = "TotalRevenue", action = "County", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "TotalRevenueTilesState",
               "Tiles/TotalRevenue/State/",
               new { controller = "TotalRevenue", action = "State", boundingEntityId = UrlParameter.Optional }
           );


           context.MapRoute(
              "Businesses",
              "Tiles/Businesses/",
              new { controller = "Businesses", action = "Index", competitorIndustryIds = UrlParameter.Optional, buyerIndustryIds = UrlParameter.Optional, supplierIndustryIds = UrlParameter.Optional }
          );




            context.MapRoute(
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
