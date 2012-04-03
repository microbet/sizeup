﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                "BusinessStateList", // Route name
                "find-business", // URL with parameters
                new { controller = "Business", action = "StateList" } // Parameter defaults
            );

            routes.MapRoute(
                "BusinessCityList", // Route name
                "find-business/{state}", // URL with parameters
                new { controller = "Business", action = "CityList", state = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "BusinessIndustryList", // Route name
                "find-business/{state}/{city}", // URL with parameters
                new { controller = "Business", action = "IndustryList", state = UrlParameter.Optional, city = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}