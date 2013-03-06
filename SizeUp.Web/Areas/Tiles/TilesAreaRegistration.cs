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
               "Businesses",
               "Tiles/Businesses/",
               new { controller = "Businesses", action = "Index", competitorIndustryIds = UrlParameter.Optional, buyerIndustryIds = UrlParameter.Optional, supplierIndustryIds = UrlParameter.Optional }
           );


            context.MapRoute(
             "GeographyBoundary",
             "Tiles/GeographyBoundary/",
             new { controller = "GeographyBoundary", action = "Index", entityId = UrlParameter.Optional }
         );




            context.MapRoute(
              "ConsumerExpendituresTilesZip",
              "Tiles/ConsumerExpenditures/Zip/",
              new { controller = "ConsumerExpenditures", action = "Zip", boundingEntityId = UrlParameter.Optional }
          );

            context.MapRoute(
               "ConsumerExpendituresTilesCounty",
               "Tiles/ConsumerExpenditures/County/",
               new { controller = "ConsumerExpenditures", action = "County", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
               "ConsumerExpendituresTilesState",
               "Tiles/ConsumerExpenditures/State/",
               new { controller = "ConsumerExpenditures", action = "State", boundingEntityId = UrlParameter.Optional }
           );

            context.MapRoute(
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
