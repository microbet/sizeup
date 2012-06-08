﻿using System.Web.Mvc;

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
                "Tiles_default",
                "Tiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}