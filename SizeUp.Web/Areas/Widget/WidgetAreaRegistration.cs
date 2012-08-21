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
                "Widget_Place",
                "Widget/{controller}/{state}/{county}/{city}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );

            context.MapRoute(
                "Widget_PlaceIndustry",
                "Widget/{controller}/{state}/{county}/{city}/{industry}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );

            context.MapRoute(
                "Widget_default",
                "Widget/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "SizeUp.Web.Areas.Widget.Controllers" }
            );
        }
    }
}
