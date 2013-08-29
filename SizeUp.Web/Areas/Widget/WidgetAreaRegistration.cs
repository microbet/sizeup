using System.Web.Mvc;

namespace SizeUp.Web.Areas.Widget
{
    public class WidgetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Widget";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Widget_getBestIndustries",
               "Widget/get/bestIndustries/{state}/{county}/{city}",
               new { controller = "Get", action = "BestIndustries", state = UrlParameter.Optional, county = UrlParameter.Optional, city = UrlParameter.Optional },
               new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
           );

            context.MapRoute(
                "Widget_Place",
                "Widget/{controller}/{state}/{county}/{city}",
                new { action = "Index" },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );

            context.MapRoute(
                "Widget_PlaceIndustry",
                "Widget/{controller}/{state}/{county}/{city}/{industry}",
                new { action = "Index" },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );

            context.MapRoute(
               "Widget_BestPlaces",
               "Widget/BestPlaces/{industry}",
               new { controller = "BestPlaces", action = "Index" },
               new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
           );

            context.MapRoute(
                "Widget_default",
                "Widget/{controller}/{action}",
                new { action = "Index" },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );
        }
    }
}
