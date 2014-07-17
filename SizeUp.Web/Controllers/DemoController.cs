using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using Microsoft.SqlServer.Types;

namespace SizeUp.Web.Controllers
{
    public class DemoController : BaseController
    {
        public string authToken = "ftyTAnI86i";
        List<string> countries = new List<string>() { "italy", "germany"};
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            var country = Request.QueryString["c"];
            var token = Request.QueryString["t"];

            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(country) || token != authToken || !countries.Contains(country) )
                return Redirect("/");

            ViewBag.Country = country;

            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion == 8)
                return View("IndexAlt");
            else
                return View();
            
        }


        public ActionResult Test()
        {
            //using (var context = ContextFactory.SizeUpContext)
            //{
            //    ViewBag.Strings = context.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);
            //    ViewBag.Header.ActiveTab = NavItems.Dashboard;
            //    return View();
            //}
            //ViewBag.Country = "italy";
            //return View();

            var country = Request.QueryString["c"];
            var token = Request.QueryString["t"];

            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion == 8)
                ViewBag.IE8 = "true";
            else
                ViewBag.IE8 = "false";

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(country) || token != authToken || !countries.Contains(country))
                return Redirect("/");

            ViewBag.Country = country;
            if (country == "italy")
            {
                ViewBag.Industry = "Shoe & Footwear Stoes";
                ViewBag.Location = "Varese";
                ViewBag.RowATitle = "Citta";
                ViewBag.RowBTitle = "Provincia";
                ViewBag.RowCTitle = "Regione";
                ViewBag.RowDTitle = "Nazione";
                ViewBag.RowA = "Varese";
                ViewBag.RowB = "Varese";
                ViewBag.RowC = "Lombardia";
                ViewBag.RowD = "Italia";
            }

            if (country == "germany")
            {
                ViewBag.Industry = "Food Products & Manufacturers";
                ViewBag.Location = "Ostalbkreis";
                ViewBag.RowATitle = "District";
                ViewBag.RowBTitle = "Region";
                ViewBag.RowCTitle = "State";
                ViewBag.RowDTitle = "Nation";
                ViewBag.RowA = "Ostalbkreis";
                ViewBag.RowB = "Stuttgart";
                ViewBag.RowC = "Baden-Wurttemberg";
                ViewBag.RowD = "Deutschland";
            }

            return View();
        }


    }
}
