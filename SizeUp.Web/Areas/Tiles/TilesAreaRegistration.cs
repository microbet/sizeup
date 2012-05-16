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
                "SalaryTilesCounty",
                "Tiles/Salary/County/",
                new { controller = "AverageSalary", action = "County", boundingEntityId = UrlParameter.Optional }
            );

            context.MapRoute(
               "SalaryTilesState",
               "Tiles/Salary/State/",
               new { controller = "AverageSalary", action = "State", boundingEntityId = UrlParameter.Optional }
           );




            context.MapRoute(
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
