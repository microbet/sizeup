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


        public ActionResult City(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range averageEmployees = ParseQueryString("averageEmployees");
            Range totalEmployees = ParseQueryString("totalEmployees");


            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Cities.Select(i => new
                {
                    City = i,
                    IndustryData = i.IndustryDataByCities.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCities.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.City.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.City.StateId == stateId.Value);
                }


                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (averageEmployees != null)
                {
                    if (averageEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees >= averageEmployees.Min);
                    }
                    if (averageEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees <= averageEmployees.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees <= totalEmployees.Max);
                    }
                }


                var raw = entities.Select(i => new
                    {
                        City = new Models.City.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            Centroid = i.City.CityGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        },
                        County = i.City.CityCountyMappings.Select(c => new Models.County.County()
                        {
                            Id = c.County.Id,
                            Name = c.County.Name,
                            SEOKey = c.County.SEOKey,
                            Centroid = c.County.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        }).FirstOrDefault(),
                        State = new Models.State.State()
                        {
                            Id = i.City.State.Id,
                            Name = i.City.State.Name,
                            Abbreviation = i.City.State.Abbreviation,
                            SEOKey = i.City.State.SEOKey,
                            Centroid = i.City.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData,
                        Demographics = i.Demographics
                    });



                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            City = i.City,
                            County = i.County,
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
                            City = i.City,
                            County = i.County,
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
                            City = i.City,
                            County = i.County,
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
                            City = i.City,
                            County = i.County,
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
                            City = i.City,
                            County = i.County,
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
                            City = i.City,
                            County = i.County,
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
      

        public ActionResult County(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {
            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range averageEmployees = ParseQueryString("averageEmployees");
            Range totalEmployees = ParseQueryString("totalEmployees");


            using (var context = ContextFactory.SizeUpContext)
            {

                var entities = context.Counties.Select(i => new
                {
                    County = i,
                    IndustryData = i.IndustryDataByCounties.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault(),
                    Demographics = i.DemographicsByCounties.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                if (regionId != null)
                {
                    entities = entities.Where(i => i.County.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.County.StateId == stateId.Value);
                }


                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (averageEmployees != null)
                {
                    if (averageEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees >= averageEmployees.Min);
                    }
                    if (averageEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees <= averageEmployees.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees <= totalEmployees.Max);
                    }
                }



                var raw = entities
                    .Select(i => new
                    {
                        County = new Models.County.County(){
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey,
                            Centroid = i.County.CountyGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault()
                        },
                        State = new Models.State.State()
                        {
                            Id = i.County.State.Id,
                            Name = i.County.State.Name,
                            Abbreviation = i.County.State.Abbreviation,
                            SEOKey = i.County.State.SEOKey,
                            Centroid = i.County.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData,
                        Demographics = i.Demographics
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            County = i.County,
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
                            County = i.County,
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
                            County = i.County,
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
                            County = i.County,
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
                            County = i.County,
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
                            County = i.County,
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

        public ActionResult Metro(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {

            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range averageEmployees = ParseQueryString("averageEmployees");
            Range totalEmployees = ParseQueryString("totalEmployees");

            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.Metroes.Select(i => new
                {
                    Metro = i,
                    IndustryData = i.IndustryDataByMetroes.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault()//,
                    //Demographics = i.de.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });

                

                if (regionId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c=>c.State.DivisionId == regionId.Value));
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.Metro.Counties.Any(c=>c.StateId == stateId.Value));
                }

                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (averageEmployees != null)
                {
                    if (averageEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees >= averageEmployees.Min);
                    }
                    if (averageEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees <= averageEmployees.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees <= totalEmployees.Max);
                    }
                }


                var raw = entities
                    .Select(i => new
                    {
                        Metro = new Models.Metro.Metro()
                        {
                            Id = i.Metro.Id,
                            Name = i.Metro.Name,
                            Centroid = i.Metro.MetroGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData
                    });

                IQueryable<Models.TopPlaces.TopPlace> data = null;
                if (attribute.Equals("totalRevenue", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = raw
                        .Where(i => i.IndustryData.TotalRevenue > 0)
                        .OrderByDescending(i => i.IndustryData.TotalRevenue)
                        .Select(i => new Models.TopPlaces.TopPlace
                        {
                            Metro = i.Metro,
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
                            Metro = i.Metro,
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
                            Metro = i.Metro,
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
                            Metro = i.Metro,
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
                            Metro = i.Metro,
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
                            Metro = i.Metro,
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

        public ActionResult State(int itemCount, int industryId, string attribute, long? regionId, long? stateId)
        {

            Range averageRevenue = ParseQueryString("averageRevenue");
            Range totalRevenue = ParseQueryString("totalRevenue");
            Range averageEmployees = ParseQueryString("averageEmployees");
            Range totalEmployees = ParseQueryString("totalEmployees");


            using (var context = ContextFactory.SizeUpContext)
            {
                var entities = context.States.Select(i => new
                {
                    State = i,
                    IndustryData = i.IndustryDataByStates.Where(id => id.Year == TimeSlice.Year && id.Quarter == TimeSlice.Quarter && id.IndustryId == industryId).FirstOrDefault()//,
                    //Demographics = i.de.Where(d => d.Year == TimeSlice.Year && d.Quarter == TimeSlice.Quarter).FirstOrDefault()
                });



                if (regionId != null)
                {
                    entities = entities.Where(i => i.State.DivisionId == regionId.Value);
                }
                if (stateId != null)
                {
                    entities = entities.Where(i => i.State.Id == stateId.Value);
                }

                if (averageRevenue != null)
                {
                    if (averageRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue >= averageRevenue.Min);
                    }
                    if (averageRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageRevenue <= averageRevenue.Max);
                    }
                }

                if (totalRevenue != null)
                {
                    if (totalRevenue.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue >= totalRevenue.Min);
                    }
                    if (totalRevenue.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalRevenue <= totalRevenue.Max);
                    }
                }

                if (averageEmployees != null)
                {
                    if (averageEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees >= averageEmployees.Min);
                    }
                    if (averageEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.AverageEmployees <= averageEmployees.Max);
                    }
                }

                if (totalEmployees != null)
                {
                    if (totalEmployees.Min.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees >= totalEmployees.Min);
                    }
                    if (totalEmployees.Max.HasValue)
                    {
                        entities = entities.Where(i => i.IndustryData.TotalEmployees <= totalEmployees.Max);
                    }
                }


                var raw = entities
                    .Select(i => new
                    {
                        State = new Models.State.State()
                        {
                            Id = i.State.Id,
                            Name = i.State.Name,
                            Abbreviation = i.State.Abbreviation,
                            SEOKey = i.State.SEOKey,
                            Centroid = i.State.StateGeographies.Where(g => g.GeographyClass.Name == "Calculation").Select(g => new Models.Shared.LatLng { Lat = g.Geography.CenterLat, Lng = g.Geography.CenterLong }).FirstOrDefault(),
                        },
                        IndustryData = i.IndustryData
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
