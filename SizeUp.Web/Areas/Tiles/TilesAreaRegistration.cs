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
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
