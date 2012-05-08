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
                "SalaryTiles",
                "Tiles/Salary/",
                new { controller = "AverageSalary", action = "Get" }
            );




            context.MapRoute(
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
