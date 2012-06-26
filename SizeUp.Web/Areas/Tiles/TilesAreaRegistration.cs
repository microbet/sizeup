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
                "RevenueTilesZip",
                "Tiles/Revenue/Zip/",
                new { controller = "Revenue", action = "Zip", boundingEntityId = UrlParameter.Optional }
            );



            context.MapRoute(
               "RevenueTilesCounty",
               "Tiles/Revenue/County/",
               new { controller = "Revenue", action = "County", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "RevenueTilesState",
               "Tiles/Revenue/State/",
               new { controller = "Revenue", action = "State", boundingEntityId = UrlParameter.Optional }
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
