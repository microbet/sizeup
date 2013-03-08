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

            /*********user***********/
            context.MapRoute(
                "UserAuthenticated",
                "Api/User/Authenticated",
                new { controller = "User", action = "Authenticated" },
                new string[] { "SizeUp.Web.Areas.Api.Controllers" }
            );








            /****advertising*****/

            context.MapRoute(
                "BestPlacesToAdvertise",
                "Api/Advertising/",
                new { controller = "Advertising", action = "Advertising" }
            );

            context.MapRoute(
                "BestPlacesToAdvertiseMinDistance",
                "Api/Advertising/MinimumDistance",
                new { controller = "Advertising", action = "MinimumDistance" }
            );

            context.MapRoute(
               "BestPlacesToAdvertiseBands",
               "Api/Advertising/Bands",
               new { controller = "Advertising", action = "Bands" }
           );





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
            context.MapRoute(
           "GetCurrentIndustry",
           "Api/Industry/Current",
           new { controller = "Industry", action = "Current" },
            new { httpMethod = new HttpMethodConstraint("GET") }
       );

            context.MapRoute(
               "SetCurrentIndustry",
               "Api/Industry/Current",
               new { controller = "Industry", action = "Current" },
                new { httpMethod = new HttpMethodConstraint("POST") }
           );


            context.MapRoute(
             "GetCurrentPlace",
             "Api/Place/Current",
             new { controller = "Place", action = "Current" },
                new { httpMethod = new HttpMethodConstraint("GET") }
         );

            context.MapRoute(
           "SetCurrentPlace",
           "Api/Place/Current",
           new { controller = "Place", action = "Current" },
                new { httpMethod = new HttpMethodConstraint("POST") }
       );









            /****consumerEpenditures*****/






            context.MapRoute(
               "BestPlacesCity",
               "Api/BestPlaces/City",
               new { controller = "BestPlaces", action = "City" },
               new string[] { "SizeUp.Web.Areas.Api.Controllers" }
           );

            context.MapRoute(
              "BestPlacesCounty",
              "Api/BestPlaces/County",
              new { controller = "BestPlaces", action = "County" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesMetro",
              "Api/BestPlaces/Metro",
              new { controller = "BestPlaces", action = "Metro" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesState",
              "Api/BestPlaces/State",
              new { controller = "BestPlaces", action = "State" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );


            context.MapRoute(
               "BestPlacesBandsCity",
               "Api/BestPlaces/Bands/City",
               new { controller = "BestPlaces", action = "CityBands" },
               new string[] { "SizeUp.Web.Areas.Api.Controllers" }
           );

            context.MapRoute(
              "BestPlacesBandsCounty",
              "Api/BestPlaces/Bands/County",
              new { controller = "BestPlaces", action = "CountyBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesBandsMetro",
              "Api/BestPlaces/Bands/Metro",
              new { controller = "BestPlaces", action = "MetroBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
          );

            context.MapRoute(
              "BestPlacesBandsState",
              "Api/BestPlaces/Bands/State",
              new { controller = "BestPlaces", action = "StateBands" },
              new string[] { "SizeUp.Web.Areas.Api.Controllers" }
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
