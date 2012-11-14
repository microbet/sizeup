using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SizeUp.Data;
using SizeUp.Core.DataAccess;
using SizeUp.Core;
using SizeUp.Web.Areas.Api.Models.TopPlaces;
using SizeUp.Core.Web;

namespace SizeUp.Web.Areas.Api.Controllers
{
    public class TopPlacesController : Controller
    {
        //
        // GET: /Api/TopPlaces/
        private Range ParseQueryString(string index)
        {
            Range v = null;
            int?[] ar = QueryString.IntValues(index);

            if (ar != null)
            {
                v = new Models.TopPlaces.Range();
                v.Min = ar[0];
                v.Max = ar[1];
            }
            return v;
        }

        public ActionResult City(int itemCount, int industryId, string attribute)
        {
            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range totalEmployees = ParseQueryString("totalEmployees");
            Range revenuePerCapita = ParseQueryString("revenuePerCapita");



            using (var context = ContextFactory.SizeUpContext)
            {
                var raw = context.Cities.Select(i =>
                new
                {
                    City = new
                    {
                        i.Id,
                        i.Name,
                        i.SEOKey,
                        LatLng = i.CityGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        County = i.CityCountyMappings.Select(c=> new
                        {
                                c.County.Id,
                                c.County.Name,
                                c.County.SEOKey
                        }).FirstOrDefault(),
                        State = new
                        {
                            i.State.Id,
                            i.State.Name,
                            i.State.SEOKey
                        }
                    },
                    Demographics = i.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).Select(d => new
                    {
                        d.UniversitiesWithin50Miles
                    }).FirstOrDefault(),
                    IndustryData = i.IndustryDataByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter && d.IndustryId == industryId).Select(d => new
                    {
                        d.TotalRevenue,
                        d.AverageRevenue,
                        d.AverageEmployees,
                        d.TotalEmployees,
                        d.EmployeesPerCapita
                    }).FirstOrDefault()

                });



                var data = raw;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.OrderByDescending(i => i.IndustryData.TotalRevenue);
                }



                var outData = data.Take(itemCount).ToList();
                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult County()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Counties.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Metro()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.Metroes.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult State()
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var data = context.States.Take(25).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
