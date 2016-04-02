using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SizeUp.Core.Diagnostics;

namespace SizeUp.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAndLogAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "FindBusinessState", // Route name
                "find-business/", // URL with parameters
                new { controller = "Business", action = "FindState" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
                "FindBusinessCounty", // Route name
                "find-business/{state}", // URL with parameters
                new { controller = "Business", action = "FindCity" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
               "FindBusinessIndustry", // Route name
               "find-business/{state}/{county}/{city}", // URL with parameters
               new { controller = "Business", action = "FindIndustry" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "FindBusiness", // Route name
               "find-business/{state}/{county}/{city}/{industry}", // URL with parameters
               new { controller = "Business", action = "Find" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
              "Business", // Route name
              "business/{state}/{county}/{city}/{industry}/{id}/{name}", // URL with parameters
              new { controller = "Business", action = "Business" }, // Parameter defaults
              new string[] { "SizeUp.Web.Controllers" }
          );



            routes.MapRoute(
                "FindCommunityState", // Route name
                "find-community/", // URL with parameters
                new { controller = "Community", action = "FindState" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
                "FindCommunityCounty", // Route name
                "find-community/{state}", // URL with parameters
                new { controller = "Community", action = "FindCity" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
               "FindCommunityIndustry", // Route name
               "find-community/{state}/{county}/{city}", // URL with parameters
               new { controller = "Community", action = "FindIndustry" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );






            routes.MapRoute(
               "BestPlaces", // Route name
               "bestPlaces/{industry}", // URL with parameters
               new { controller = "BestPlaces", action = "Index" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "BestPlacesPickIndustry", // Route name
               "bestPlaces/", // URL with parameters
               new { controller = "BestPlaces", action = "PickIndustry" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );


            routes.MapRoute(
                "Advertising", // Route name
                "advertising/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Advertising", action = "Index" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );


            routes.MapRoute(
                "Competition", // Route name
                "competition/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Competition", action = "Index" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
                "Dashboard", // Route name
                "dashboard/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Dashboard", action = "Index" }, // Parameter defaults
                new string[] { "SizeUp.Web.Controllers" }
            );

            routes.MapRoute(
               "CityCommunity", // Route name
               "community/{state}/{county}/{city}", // URL with parameters
               new { controller = "Community", action = "CityCommunity" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "CountyCommunity", // Route name
               "community/{state}/{county}", // URL with parameters
               new { controller = "Community", action = "CountyCommunity" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "MetroCommunity", // Route name
               "communitymetro/{metro}", // URL with parameters
               new { controller = "Community", action = "MetroCommunity" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "StateCommunity", // Route name
               "community/{state}", // URL with parameters
               new { controller = "Community", action = "StateCommunity" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );


            routes.MapRoute(
               "CommunityWithIndustry", // Route name
               "community/{state}/{county}/{city}/{industry}", // URL with parameters
               new { controller = "Community", action = "CommunityWithIndustry" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );


            routes.MapRoute(
               "Redirect", // Route name
               "community/{oldSEO}", // URL with parameters
               new { controller = "Community", action = "Redirect" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "RedirectWithIndustry", // Route name
               "community/{oldSEO}/{industry}", // URL with parameters
               new { controller = "Community", action = "RedirectWithIndustry" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );

            routes.MapRoute(
               "RedirectBusiness", // Route name
               "business/{oldSEO}", // URL with parameters
               new { controller = "Business", action = "Redirect" }, // Parameter defaults
               new string[] { "SizeUp.Web.Controllers" }
           );


            routes.MapRoute(
              "SigninRedirect", // Route name
              "signin", // URL with parameters
              new { controller = "User", action = "SigninRedirect" }, // Parameter defaults
              new string[] { "SizeUp.Web.Controllers" }
          );


        


            routes.MapRoute(
                "404", // Route name
                "error/404", // URL with parameters
                new { controller = "Error", action = "Error404" } // Parameter defaults
            );

            routes.MapRoute(
               "500", // Route name
               "error/500", // URL with parameters
               new { controller = "Error", action = "Error500" } // Parameter defaults
           );


            routes.MapRoute(
              "accessibility", // Route name
              "accessibility/{action}/{levelOfDetail}", // URL with parameters
              new { controller = "Accessibility" } // Parameter defaults
          );


        routes.MapRoute(
           "sitemap", // Route name
           "sitemap.xml", // URL with parameters
           new { controller = "SiteMap", action ="Index" } // Parameter defaults
       );


            /*
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/


            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "SizeUp.Web.Controllers" }
            );
        }

        public static void RegisterBundles()
        {
            // BundleTable.Bundles.EnableDefaultBundles();


            /*
          var jSBundle = new Bundle("~/js/JsMinify", typeof(JsMinify));

 

          jSBundle.AddFile("~/Scripts/CustomFunction.js");

          jSBundle.AddFile("~/Scripts/jquery-1.4.1-vsdoc.js");

          jSBundle.AddFile("~/Scripts/jquery-1.4.1.js");

          jSBundle.AddFile("~/Scripts/JSONCreate.js");

          BundleTable.Bundles.Add(jSBundle);

          var cssBundle = new Bundle("~/CSSMinify", typeof(CssMinify));

 

          cssBundle.AddFile("~/Styles/Collection.css");

          cssBundle.AddFile("~/Styles/GlobalSupport.css");

          cssBundle.AddFile("~/Styles/MasterStyle.css");

          cssBundle.AddFile("~/Styles/MenuStyle.css");

          cssBundle.AddFile("~/Styles/Minimum.css");

          cssBundle.AddFile("~/Styles/Ribbon.css");

          cssBundle.AddFile("~/Styles/Site.css");

          BundleTable.Bundles.Add(cssBundle);

 */

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory("Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            RegisterBundles();
        }
    }
}