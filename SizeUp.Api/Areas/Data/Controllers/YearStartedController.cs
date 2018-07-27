using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core;
using SizeUp.Core.Web;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer;
using SizeUp.Core.API;
using SizeUp.Api.Controllers;
namespace SizeUp.Api.Areas.Data.Controllers
{
    public class YearStartedController : BaseController
    {
        public class Table
        {
            public int Year { get; set; }
            public int City { get; set; }
            public int? County { get; set; }
            public int? Metro { get; set; }
            public int? State { get; set; }
            public int? Nation { get; set; }
        }

        //
        // GET: /Api/YearStarted/
        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Chart(long industryId, long geographicLocationId, int startYear, int endYear, long placeId = -1, string contentType = "*/*")
        {
            if (startYear < 1986)
            {
                startYear = 1986;
            }

            using (var context = ContextFactory.SizeUpContext)
            {
                if (geographicLocationId != -1)
                {
                    var obj = Core.DataLayer.YearStarted.Chart(context, industryId, geographicLocationId, startYear, endYear);
                    if ("text/html".Equals(contentType))
                    {
                        SizeUp.Data.GeographicLocation location = Core.DataLayer.GeographicLocation.Get(context)
                            .Where(i => i.Id == geographicLocationId)
                            .FirstOrDefault();
                        List<Table> data = new List<Table>();
                        foreach (var i in obj) {
                            // TODO - we don't know that geographicLocationId is a city. Check location.GranularityId.
                            data.Add(new Table() { Year = i.Key, City = i.Value });
                        }
                        ViewBag.Data = data;
                        ViewBag.Industry = context.Industries
                            .Where(i => i.Id == industryId)
                            .FirstOrDefault();
                        ViewBag.Location = location;
                        ViewBag.TableColumns = new Table() { Year = 1, City = 1 };
                        ViewBag.ThemeUrl = System.Configuration.ConfigurationManager.AppSettings["Theme.Url"];

                        ViewBag.Query = string.Format(
                            "How many {0} businesses started in {1} during each year from {2} to {3}?",
                            ViewBag.Industry.Name, ViewBag.Location.LongName, startYear, endYear
                        );
                        return View("Linechart");
                    }
                    else
                    {
                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (placeId != -1)
                {
                    var place = Core.DataLayer.Place.Get(context, placeId);

                    var c = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.City.Id, startYear, endYear);
                    var co = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.County.Id, startYear, endYear);
                    var s = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.State.Id, startYear, endYear);
                    var n = Core.DataLayer.YearStarted.Chart(context, industryId, place.Nation.Id, startYear, endYear);

                    List<Table> data =
                        c.Join(co, i => i.Key, o => o.Key, (i, o) => new { City = i, County = o })
                        .Join(s, i => i.City.Key, o => o.Key, (i, o) => new { City = i.City, County = i.County, State = o })
                        .Join(n, i => i.City.Key, o => o.Key, (i, o) => new Table() { Year = i.City.Key, City = i.City.Value, County = i.County.Value, State = i.State.Value, Nation = o.Value })
                        .ToList();

                    ViewBag.TableColumns = new Table() { Year = 1, City = 1, County = 1, State = 1, Nation = 1 };

                    if (place.Metro.Id.HasValue)
                    {
                        var m = Core.DataLayer.YearStarted.Chart(context, industryId, (long)place.Metro.Id, startYear, endYear);
                        data = data.Join(m, i => i.Year, o => o.Key, (i, o) => new Table() { Year = i.Year, City = i.City, County = i.County, State = i.State, Nation = i.Nation, Metro = o.Value })
                            .ToList();
                        ViewBag.TableColumns.Metro = 1;
                    }

                    ViewBag.Data = data;
                    ViewBag.EndYear = endYear;
                    ViewBag.Industry = context.Industries
                        .Where(i => i.Id == industryId)
                        .FirstOrDefault();
                    ViewBag.Place = place;
                    ViewBag.StartYear = startYear;
                    ViewBag.ThemeUrl = System.Configuration.ConfigurationManager.AppSettings["Theme.Url"];

                    ViewBag.Query = string.Format(
                        "How many {0} businesses started in {1}, {2} during each year from {3} to {4}?",
                        ViewBag.Industry.Name, ViewBag.Place.City.Name, ViewBag.Place.State.Abbreviation, ViewBag.StartYear, ViewBag.EndYear
                    );
                    return View("Linechart");
                }
                else
                {
                    HttpContext.Response.StatusCode = 400;
                    return Content("geographicLocationId and placeId were both -1. One must be set.");
                }
            }

        }

        
        [APIAuthorize(Role = "IndustryData")]
        public ActionResult Percentile(long industryId, long geographicLocationId, int value)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var obj = Core.DataLayer.YearStarted.Percentile(context, industryId, geographicLocationId, value);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
