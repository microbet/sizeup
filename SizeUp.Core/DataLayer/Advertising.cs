using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class Advertising 
    {
        protected static IQueryable<DistanceEntity<Models.Advertising>> FilterQuery(IQueryable<DistanceEntity<Models.Advertising>> data, AdvertisingFilters filters)
        {
            if (filters.AverageRevenue != null)
            {
                if (filters.AverageRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.AverageRevenue >= filters.AverageRevenue.Min);
                }
                if (filters.AverageRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.AverageRevenue <= filters.AverageRevenue.Max);
                }
            }

            if (filters.TotalRevenue != null)
            {
                if (filters.TotalRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.TotalRevenue >= filters.TotalRevenue.Min);
                }
                if (filters.TotalRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.TotalRevenue <= filters.TotalRevenue.Max);
                }
            }

            if (filters.TotalEmployees != null)
            {
                if (filters.TotalEmployees.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.TotalEmployees >= filters.TotalEmployees.Min);
                }
                if (filters.TotalEmployees.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.TotalEmployees <= filters.TotalEmployees.Max);
                }
            }

            if (filters.RevenuePerCapita != null)
            {
                if (filters.RevenuePerCapita.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.RevenuePerCapita >= filters.RevenuePerCapita.Min);
                }
                if (filters.RevenuePerCapita.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.RevenuePerCapita <= filters.RevenuePerCapita.Max);
                }
            }

            if (filters.HouseholdIncome != null)
            {
                if (filters.HouseholdIncome.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.HouseholdIncome >= filters.HouseholdIncome.Min);
                }
                if (filters.HouseholdIncome.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.HouseholdIncome <= filters.HouseholdIncome.Max);
                }
            }

            if (filters.HouseholdExpenditures != null)
            {
                if (filters.HouseholdExpenditures.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.HouseholdExpenditures >= filters.HouseholdExpenditures.Min);
                }
                if (filters.HouseholdExpenditures.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.HouseholdExpenditures <= filters.HouseholdExpenditures.Max);
                }
            }

            if (filters.MedianAge != null)
            {
                if (filters.MedianAge.Min.HasValue)
                {
                    data = data.Where(i => i.Entity.MedianAge >= filters.MedianAge.Min);
                }
                if (filters.MedianAge.Max.HasValue)
                {
                    data = data.Where(i => i.Entity.MedianAge <= filters.MedianAge.Max);
                }
            }

            if (filters.BachelorOrHigher != null)
            {
                var v = filters.BachelorOrHigher / 100.0d;
                data = data.Where(i => i.Entity.BachelorsDegreeOrHigher >= v);
            }

            if (filters.HighSchoolOrHigher != null)
            {
                var v = filters.HighSchoolOrHigher / 100.0d;
                data = data.Where(i => i.Entity.HighSchoolOrHigher >= v);
            }

            if (filters.WhiteCollarWorkers != null)
            {
                var v = filters.WhiteCollarWorkers / 100.0d;
                data = data.Where(i => i.Entity.WhiteCollarWorkers >= v);
            }


            if (filters.Distance != null)
            {
                data = data.Where(i => i.Distance < filters.Distance.Value);
            }
            if (filters.Attribute == "totalRevenue")
            {
                data = data.Where(i => i.Entity.TotalRevenue != null && i.Entity.TotalRevenue > 0);
            }
            else if (filters.Attribute == "averageRevenue")
            {
                data = data.Where(i => i.Entity.AverageRevenue != null && i.Entity.AverageRevenue > 0);
            }
            else if (filters.Attribute == "revenuePerCapita")
            {
                data = data.Where(i => i.Entity.RevenuePerCapita != null && i.Entity.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "underservedMarkets")
            {
                data = data.Where(i => i.Entity.RevenuePerCapita != null && i.Entity.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "householdIncome")
            {
                data = data.Where(i => i.Entity.HouseholdIncome != null && i.Entity.HouseholdIncome > 0);
            }
            else if (filters.Attribute == "totalPopulation")
            {
                data = data.Where(i => i.Entity.Population != null && i.Entity.Population > 0);
            }
            else if (filters.Attribute == "whiteCollarWorkers")
            {
                data = data.Where(i => i.Entity.WhiteCollarWorkers != null && i.Entity.WhiteCollarWorkers > 0);
            }
            else if (filters.Attribute == "totalEmployees")
            {
                data = data.Where(i => i.Entity.TotalEmployees != null && i.Entity.TotalEmployees > 0);
            }
            else if (filters.Attribute == "householdExpenditures")
            {
                data = data.Where(i => i.Entity.HouseholdExpenditures != null && i.Entity.HouseholdExpenditures > 0);
            }
            else if (filters.Attribute == "medianAge")
            {
                data = data.Where(i => i.Entity.MedianAge != null && i.Entity.MedianAge > 0);
            }
            else if (filters.Attribute == "bachelorsDegreeOrHigher")
            {
                data = data.Where(i => i.Entity.BachelorsDegreeOrHigher != null && i.Entity.BachelorsDegreeOrHigher > 0);
            }
            else if (filters.Attribute == "highSchoolOrHigher")
            {
                data = data.Where(i => i.Entity.HighSchoolOrHigher != null && i.Entity.HighSchoolOrHigher > 0);
            }


            switch (filters.SortAttribute)
            {
                case "name":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.ZipCode.Name);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.ZipCode.Name);
                    }
                    break;
                case "totalRevenue":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.TotalRevenue);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.TotalRevenue);
                    }
                    break;
                case "averageRevenue":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.AverageRevenue);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.AverageRevenue);
                    }
                    break;
                case "revenuePerCapita":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.RevenuePerCapita);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.RevenuePerCapita);
                    }
                    break;
                case "underservedMarkets":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.RevenuePerCapita);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.RevenuePerCapita);
                    }
                    break;
                case "householdIncome":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.HouseholdIncome);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.HouseholdIncome);
                    }
                    break;

                case "totalPopulation":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.Population);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.Population);
                    }
                    break;

                case "whiteCollarWorkers":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.WhiteCollarWorkers);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.WhiteCollarWorkers);
                    }
                    break;

                case "totalEmployees":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.TotalEmployees);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.TotalEmployees);
                    }
                    break;
                case "householdExpenditures":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.HouseholdExpenditures);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.HouseholdExpenditures);
                    }
                    break;
                case "medianAge":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.MedianAge);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.MedianAge);
                    }
                    break;
                case "bachelorsDegreeOrHigher":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.BachelorsDegreeOrHigher);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.BachelorsDegreeOrHigher);
                    }
                    break;
                case "highSchoolOrHigher":
                    if (filters.Sort == "desc")
                    {
                        data = data.OrderByDescending(i => i.Entity.HighSchoolOrHigher);
                    }
                    else
                    {
                        data = data.OrderBy(i => i.Entity.HighSchoolOrHigher);
                    }
                    break;

                default:
                    data = data.OrderBy(i => i.Entity.ZipCode.Name);
                    break;

            }
            return data;
        }

        public static IQueryable<Models.AdvertisingOutput> Get(SizeUpContext context, long industryId, long placeId, AdvertisingFilters filters)
        {
            var center = Core.DataLayer.Geography.Display(context).Where(i => i.GeographicLocationId == placeId).Select(new Projections.Geography.Centroid().Expression).FirstOrDefault();
            DistanceEntity<Data.ZipCode>.DistanceEntityFilter dist = new DistanceEntity<Data.ZipCode>.DistanceEntityFilter(center);

            var zips = Core.DataLayer.GeographicLocation.Get(context, Granularity.ZipCode)
                .Select(i => new KeyValue<Data.ZipCode, Geo.LatLng>
                {
                    Key = i.ZipCode,
                    Value = i.Geographies.Where(g => g.GeographyClass.Name == Geo.GeographyClass.Calculation).Select(g => new Geo.LatLng
                    {
                        Lat = g.CenterLat.Value,
                        Lng = g.CenterLong.Value
                    }).FirstOrDefault()
                })
                .Select(dist.Projection);

            var demographics = Core.DataLayer.Demographics.Get(context, Granularity.ZipCode);
            var industry = Core.DataLayer.IndustryData.Get(context).Where(i => i.IndustryId == industryId);
            var places = Core.DataLayer.Place.List(context);
            var data = zips.Join(demographics, i => i.Entity.Id, o => o.GeographicLocationId, (i, o) => new { Demographics = o, Entity = i })
            .Join(industry, i => i.Entity.Entity.Id, o => o.GeographicLocationId, (i, o) => new { Demographics = i.Demographics, IndustryData = o, Entity = i.Entity, Place = places.Where(p=>i.Entity.Entity.Places.Any(zm=>zm.Id == p.Id)).FirstOrDefault() })
            .Select(i => new DistanceEntity<Models.Advertising>
            {
                Distance = i.Entity.Distance,
                Entity = new Models.Advertising
                {
                    Place = i.Place,
                    ZipCode = new Models.ZipCode
                    {
                        Id = i.Entity.Entity.Id,
                        Name = i.Entity.Entity.Name,
                        Zip = i.Entity.Entity.Zip
                    },
                    Centroid = i.Entity.Entity.GeographicLocation.Geographies.Where(zg=>zg.GeographyClass.Name == Geo.GeographyClass.Calculation).Select(zg=> new Geo.LatLng
                    {
                        Lat = zg.CenterLat.Value,
                        Lng = zg.CenterLong.Value
                    }).FirstOrDefault(),                  
                    AverageRevenue = i.IndustryData.AverageRevenue,
                    TotalRevenue = i.IndustryData.TotalRevenue,
                    TotalEmployees = i.IndustryData.TotalEmployees,
                    RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                    BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                    HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                    HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                    WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                    MedianAge = i.Demographics.MedianAge,
                    Population = i.Demographics.TotalPopulation,
                    HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,

                    TotalEmployeesBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),                  
                    AverageRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                    TotalRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                    RevenuePerCapitaBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault()


                }
            });
            data = FilterQuery(data, filters);
            IQueryable<Models.AdvertisingOutput> output = new List<Models.AdvertisingOutput>().AsQueryable();
            output = data.Select(i => new Models.AdvertisingOutput
            {
                Place = i.Entity.Place,
                Centroid = i.Entity.Centroid,
                TotalEmployees = i.Entity.TotalEmployeesBand,
                TotalRevenue = i.Entity.TotalRevenueBand,
                AverageRevenue = i.Entity.AverageRevenueBand,
                RevenuePerCapita = i.Entity.RevenuePerCapitaBand,
                BachelorsDegreeOrHigher = i.Entity.BachelorsDegreeOrHigher,
                HighSchoolOrHigher = i.Entity.HighSchoolOrHigher,
                HouseholdExpenditures = i.Entity.HouseholdExpenditures,
                HouseholdIncome = i.Entity.HouseholdIncome,
                MedianAge = i.Entity.MedianAge,
                Population = i.Entity.Population,
                WhiteCollarWorkers = i.Entity.WhiteCollarWorkers,
                ZipCode = i.Entity.ZipCode,
                Distance = i.Distance
            });
            return output;
        }

        public static int MinimumDistance(SizeUpContext context, long industryId, long placeId, int items, AdvertisingFilters filters)
        {
            var data = Get(context, industryId, placeId, filters);
            var results = data.Select(i => new
            {
                i.Distance
            });

            results = results.OrderBy(i => i.Distance);
            var distance = results.Skip(items - 1).FirstOrDefault();
            int miles = 20;
            if (distance != null)
            {
                miles = (int)System.Math.Ceiling(distance.Distance);
            }
            return miles;
        }


        public static List<Band<double>> Bands(SizeUpContext context, long industryId, long placeId, int bands, AdvertisingFilters filters)
        {
            var data = Get(context, industryId, placeId, filters);
            List<Band<double>> output = null;
            if (filters.Attribute == "totalRevenue")
            {
                output = data
                    .Select(i=>i.TotalRevenue)
                    .ToList()
                    .NTileDescending(i => i.Max, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Min), Max = b.Max(i => i.Max) })
                    .ToList();
            }
            else if (filters.Attribute == "averageRevenue")
            {
                output = data
                    .Select(i => i.AverageRevenue)
                    .ToList()
                    .NTileDescending(i => i.Max, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Min), Max = b.Max(i => i.Max) })
                    .ToList();
            }
            else if (filters.Attribute == "revenuePerCapita")
            {
                output = data
                    .Select(i => i.RevenuePerCapita)
                    .ToList()
                    .NTileDescending(i => i.Max, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Min), Max = b.Max(i => i.Max) })
                    .ToList();
            }
            else if (filters.Attribute == "underservedMarkets")
            {
                output = data
                    .Select(i => i.RevenuePerCapita)
                    .ToList()
                    .NTile(i => i.Max, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Min), Max = b.Max(i => i.Max) })
                    .ToList();
            }
            else if (filters.Attribute == "householdIncome")
            {
                output = data
                    .Select(i => i.HouseholdIncome)
                     .ToList()
                     .NTileDescending(i => i, bands)
                     .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                     .ToList();
            }
            else if (filters.Attribute == "totalPopulation")
            {
                output = data
                    .Select(i => i.Population)
                    .ToList()
                    .NTileDescending(i => i, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                    .ToList();
            }
            else if (filters.Attribute == "whiteCollarWorkers")
            {
                output = data
                    .Select(i => i.WhiteCollarWorkers)
                    .ToList()
                    .NTileDescending(i => i, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                    .ToList();
            }
            else if (filters.Attribute == "totalEmployees")
            {
                output = data
                    .Select(i => i.TotalEmployees)
                    .ToList()
                    .NTileDescending(i => i.Max, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Min), Max = b.Max(i => i.Max) })
                    .ToList();
            }
            else if (filters.Attribute == "householdExpenditures")
            {
                output = data
                    .Select(i => i.HouseholdExpenditures)
                    .ToList()
                    .NTileDescending(i => i, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                    .ToList();
            }
            else if (filters.Attribute == "medianAge")
            {
                output = data
                    .Select(i => i.MedianAge)
                    .ToList()
                    .NTileDescending(i => i, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                    .ToList();
            }
            else if (filters.Attribute == "bachelorsDegreeOrHigher")
            {
                output = data
                    .Select(i => i.BachelorsDegreeOrHigher)
                    .ToList()
                    .NTileDescending(i => i, bands)
                    .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                    .ToList();
            }
            else if (filters.Attribute == "highSchoolOrHigher")
            {
                output = data
                    .Select(i => i.HighSchoolOrHigher)
                     .ToList()
                     .NTileDescending(i => i, bands)
                     .Select(b => new Band<double>() { Min = b.Min(i => i.Value), Max = b.Max(i => i.Value) })
                     .ToList();
            }


            if (filters.Attribute == "underservedMarkets")
            {
                output.Format();
            }
            else
            {
                output.FormatDescending();
            }
            return output;
        }
    }
}
