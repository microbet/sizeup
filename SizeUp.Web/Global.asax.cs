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

namespace SizeUp.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "FindBusiness", // Route name
                "find-business/", // URL with parameters
                new { controller = "Business", action = "Find" } // Parameter defaults
            );


            routes.MapRoute(
               "TopPlaces", // Route name
               "topplaces/{industry}", // URL with parameters
               new { controller = "TopPlaces", action = "Index" } // Parameter defaults
           );


            routes.MapRoute(
                "Advertising", // Route name
                "advertising/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Advertising", action = "Index" } // Parameter defaults
            );


            routes.MapRoute(
                "Competition", // Route name
                "competition/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Competition", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Dashboard", // Route name
                "dashboard/{state}/{county}/{city}/{industry}", // URL with parameters
                new { controller = "Dashboard", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "404", // Route name
                "error/404", // URL with parameters
                new { controller = "Error", action = "Error404" } // Parameter defaults
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