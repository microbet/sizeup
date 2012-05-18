using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Web.Models;
using SizeUp.Data;
using SizeUp.Core.Web;
using SizeUp.Web.Areas.Api.Models;
using Microsoft.SqlServer.Types;

namespace SizeUp.Web.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/

        public ActionResult Index(string state, string city, string industry)
        {
            ViewBag.Strings = DataContexts.SizeUpContext.ResourceStrings.Where(i => i.Name.StartsWith("Dashboard")).ToDictionary(i => i.Name, i => i.Value);
            var ind = DataContexts.SizeUpContext.Industries.Where(i => i.SEOKey == industry)
                .Select(i=> new {
                    Industry = i,
                    NAICS6 = DataContexts.SizeUpContext.SicToNAICSMappings.Where(m=>m.IndustryId == i.Id).Select(m=>m.NAICS).FirstOrDefault(),
                    NAICS4 = DataContexts.SizeUpContext.NAICS.Where(n => n.NAICSCode == DataContexts.SizeUpContext.SicToNAICSMappings
                        .Where(m => m.IndustryId == i.Id)
                        .Select(m => m.NAICS)
                        .FirstOrDefault()
                        .NAICSCode.Substring(0,4)
                   ).FirstOrDefault()
                })
                .FirstOrDefault();
    
            var locations = DataContexts.SizeUpContext.Cities.Where(i => i.SEOKey == city && i.State.Abbreviation == state).Select(i => new { City = i, County = i.County, Metro = i.County.Metro, State = i.State }).FirstOrDefault();
            WebContext.Current.CurrentCity = locations.City;
            WebContext.Current.CurrentIndustry = ind.Industry;

            var center = new Areas.Api.Models.Maps.LatLng();
            var geo = SqlGeography.Parse(locations.City.Geography.AsText());
            var geom = SqlGeometry.STGeomFromWKB(geo.STAsBinary(), (int)geo.STSrid);
            geom = geom.STCentroid();
            geo = SqlGeography.Parse(geom.STAsText().ToSqlString());
            center.Lat = (double)geo.STPointN(1).Lat;
            center.Lng = (double)geo.STPointN(1).Long;


            ViewBag.Report = new Models.Business.Report()
            {
                Locations = new Models.Business.Locations()
                {
                    City = new Areas.Api.Models.City.City()
                    {
                        Id = locations.City.Id,
                        Name = locations.City.Name,
                        County = locations.County.Name,
                        State = locations.State.Abbreviation,
                        SEOKey = locations.City.SEOKey
                    },
                    County = new Areas.Api.Models.County.County()
                    {
                        Id = locations.County.Id,
                        Name = locations.County.Name,
                        State = locations.State.Abbreviation
                    },
                    State = new Areas.Api.Models.State.State()
                    {
                        Id = locations.State.Id,
                        Name = locations.State.Name,
                        Abbreviation = locations.State.Abbreviation,
                        SEOKey = locations.State.SEOKey
                    }
                },
                IndustryDetails = new Models.Business.IndustryDetails()
                {
                    Industry = new Areas.Api.Models.Industry.Industry()
                    {
                        Id = ind.Industry.Id,
                        Name = ind.Industry.Name,
                        SEOKey = ind.Industry.SEOKey
                    },
                    NAICS6 = new Areas.Api.Models.NAICS.NAICS()
                    {
                        Id = ind.NAICS6.Id,
                        NAICSCode = ind.NAICS6.NAICSCode,
                        Name = ind.NAICS6.Name
                    },
                    NAICS4 = new Areas.Api.Models.NAICS.NAICS()
                    {
                        Id = ind.NAICS4.Id,
                        NAICSCode = ind.NAICS4.NAICSCode,
                        Name = ind.NAICS4.Name
                    }
                },
                MapCenter = center
            };

            if (locations.Metro != null)
            {
                ViewBag.Report.Locations.Metro = new Areas.Api.Models.Metro.Metro()
                {
                    Id = locations.Metro.Id,
                    Name = locations.Metro.Name
                };
            }

            ViewBag.ReportJSON = SizeUp.Core.Serialization.Serializer.ToJSON(ViewBag.Report);
            ViewBag.Header.ActiveTab = NavItems.Dashboard;
            return View();
        }

    }
}
