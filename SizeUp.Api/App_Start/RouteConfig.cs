using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SizeUp.Api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "403", // Route name
                "error/403", // URL with parameters
                new { controller = "Error", action = "Error403" } // Parameter defaults
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

            // Serve documentation from static file(s) -- REVISIT if necessary
            routes.IgnoreRoute("documentation");
            routes.IgnoreRoute("documentation/{*file}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new {  action = "Index" }
            );
        }
    }
}