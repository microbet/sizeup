using System.Web.Mvc;
using System.Web.Routing;

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


            /****profile*****/
            //these are required
            context.MapRoute(
                "DashboardValuesGet",
                "Api/profile/dashboardValues",
                new { controller = "Profile", action = "GetDashboardValues" },
                 new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
               "DashboardValuesSet",
               "Api/profile/dashboardValues",
               new { controller = "Profile", action = "SetDashboardValues" },
                new { httpMethod = new HttpMethodConstraint("POST") }
           );

            context.MapRoute(
                "CompetitionValuesGet",
                "Api/profile/competitionValues",
                new { controller = "Profile", action = "GetCompetitionValues" },
                 new { httpMethod = new HttpMethodConstraint("GET") }
            );

            context.MapRoute(
               "CompetitionValuesSet",
               "Api/profile/competitionValues",
               new { controller = "Profile", action = "SetCompetitionValues" },
                new { httpMethod = new HttpMethodConstraint("POST") }
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
