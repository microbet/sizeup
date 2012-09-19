using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
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
               "TopPlaces", // Route name
               "topplaces/{industry}", // URL with parameters
               new { controller = "TopPlaces", action = "Index" } // Parameter defaults
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
               "Community", // Route name
               "community/{state}/{county}/{city}", // URL with parameters
               new { controller = "Community", action = "Community" } // Parameter defaults
           );

            routes.MapRoute(
               "CommunityWithIndustry", // Route name
               "community/{state}/{county}/{city}/{industry}", // URL with parameters
               new { controller = "Community", action = "CommunityWithIndustry" } // Parameter defaults
           );


            routes.MapRoute(
               "Redirect", // Route name
               "community/{oldSEO}", // URL with parameters
               new { controller = "Community", action = "Redirect" } // Parameter defaults
           );

            routes.MapRoute(
               "RedirectWithIndustry", // Route name
               "community/{oldSEO}/{industry}", // URL with parameters
               new { controller = "Community", action = "RedirectWithIndustry" } // Parameter defaults
           );

            routes.MapRoute(
                "404", // Route name
                "error/404", // URL with parameters
                new { controller = "Error", action = "Error404" } // Parameter defaults
            );


            routes.MapRoute(
              "accessibility", // Route name
              "accessibility/{action}/{levelOfDetail}", // URL with parameters
              new { controller = "Accessibility" } // Parameter defaults
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
                new string[]{"SizeUp.Web.Controllers"}
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

            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }
}