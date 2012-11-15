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



                var outData = data
                .Take(itemCount)
                .ToList()
                .Select((i, index) => new
                {
                    i.City,
                    i.IndustryData.TotalRevenue,
                    i.IndustryData.TotalEmployees,
                    i.IndustryData.EmployeesPerCapita,
                    i.IndustryData.AverageRevenue,
                    i.IndustryData.AverageEmployees,
                    Rank = index + 1
                })
                .ToList();
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

        public ActionResult State(int itemCount, int industryId, string attribute)
        {
            using (var context = ContextFactory.SizeUpContext)
            {
                var raw = context.States
                    .Select(i => new
                    {
                        State = new Models.State.State()
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Abbreviation = i.Abbreviation,
                            SEOKey = i.SEOKey,
                            Centroid = i.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng{ Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryDataByStates.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter && d.IndustryId == industryId).Select(d => new
                        {
                            d.TotalRevenue,
                            d.AverageRevenue,
                            d.TotalEmployees,
                            d.AverageEmployees,
                            d.EmployeesPerCapita,
                            d.RevenuePerCapita
                        }).FirstOrDefault()
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i=>i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.AverageRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("totalEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.TotalEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("averageEmployees", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.AverageEmployees > 0)
                        .OrderByDescending(i => i.IndustryData.AverageEmployees)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("employeesPerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.EmployeesPerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.EmployeesPerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }
                else if (attribute.Equals("revenuePerCapita", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.RevenuePerCapita > 0)
                        .OrderByDescending(i => i.IndustryData.RevenuePerCapita)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            State = i.State,
                            TotalRevenue = i.IndustryData.TotalRevenue,
                            TotalEmployees = i.IndustryData.TotalEmployees,
                            EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
                            RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                            AverageRevenue = i.IndustryData.AverageRevenue,
                            AverageEmployees = i.IndustryData.AverageEmployees
                        })
                        .AsQueryable();
                }


                var outData = data
                    .Take(itemCount)
                    .ToList();
                    

                return Json(outData, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
