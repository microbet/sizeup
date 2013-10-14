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
        protected class Wrapper
        {
            public Data.Place Place { get; set; }
            public Models.ZipCode ZipCode { get; set; }
            public Core.Geo.LatLng Centroid { get; set; }
            public Core.Geo.BoundingBox BoundingBox { get; set; }
            public long? AverageRevenue { get; set; }
            public long? TotalRevenue { get; set; }
            public long? TotalEmployees { get; set; }
            public long? RevenuePerCapita { get; set; }
            public long? HouseholdIncome { get; set; }
            public long? Population { get; set; }
            public double? BachelorsDegreeOrHigher { get; set; }
            public double? HighSchoolOrHigher { get; set; }
            public double? WhiteCollarWorkers { get; set; }
            public double? MedianAge { get; set; }
            public double? HouseholdExpenditures { get; set; }


            public Band<double> AverageRevenueBand { get; set; }
            public Band<double> TotalRevenueBand { get; set; }
            public Band<double> TotalEmployeesBand { get; set; }
            public Band<double> RevenuePerCapitaBand { get; set; }
        }

        protected class Grouping
        {
            public Data.Place Place { get; set; }
            public Data.IndustryData IndustryData { get; set; }
            public Data.Demographic Demographics { get; set; }
            public Data.ZipCode ZipCode { get; set; }
            public Data.Geography Geography { get; set; }
            public Band<double> AverageRevenueBand { get; set; }
            public Band<double> TotalRevenueBand { get; set; }
            public Band<double> TotalEmployeesBand { get; set; }
            public Band<double> RevenuePerCapitaBand { get; set; }
        }

        protected static IQueryable<DistanceEntity<Wrapper>> FilterQuery(IQueryable<DistanceEntity<Wrapper>> data, AdvertisingFilters filters)
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

        public static IQueryable<Models.Advertising> Get(SizeUpContext context, long industryId, long placeId, AdvertisingFilters filters)
        {
            var center = Core.DataLayer.Geography.Display(context)
                .Where(i => i.GeographicLocationId == placeId)
                .Select(new Projections.Geography.Centroid().Expression)
                .Select(i=>i.Value)
                .FirstOrDefault();

            DistanceEntity<Grouping>.DistanceEntityFilter dist = new DistanceEntity<Grouping>.DistanceEntityFilter(center);

            var data = context.ZipCodes
                .SelectMany(i => i.GeographicLocation.Demographics, (i, o) => new { ZipCode = i, Demographics = o })
                .SelectMany(i => i.ZipCode.GeographicLocation.IndustryDatas, (i, o) => new { i.ZipCode, i.Demographics, IndustryData = o })
                .SelectMany(i => i.ZipCode.GeographicLocation.Geographies, (i, o) => new { i.ZipCode, i.Demographics, i.IndustryData, Geography = o, Place = i.ZipCode.Places.FirstOrDefault() })
                .Where(i => i.Demographics.Year == CommonFilters.TimeSlice.Demographics.Year && i.Demographics.Quarter == CommonFilters.TimeSlice.Demographics.Quarter)
                .Where(i => i.IndustryData.Year == CommonFilters.TimeSlice.Industry.Year && i.IndustryData.Quarter == CommonFilters.TimeSlice.Industry.Quarter && i.IndustryData.IndustryId == industryId)
                .Where(i => i.Geography.GeographyClass.Name == Geo.GeographyClass.Calculation)
                .Select(i => new KeyValue<Grouping, Geo.LatLng>
                {
                    Key = new Grouping
                    {
                        Place = i.Place,
                        IndustryData = i.IndustryData,
                        Demographics = i.Demographics,
                        Geography = i.Geography,
                        ZipCode = i.ZipCode,
                        TotalEmployeesBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault()
                    },
                    Value = new Geo.LatLng
                    {
                        Lat = i.Geography.CenterLat.Value,
                        Lng = i.Geography.CenterLong.Value
                    }
                })
                .Select(dist.Projection)
                .Select(i => new { i.Distance, i.Entity.Demographics, i.Entity.Geography, i.Entity.IndustryData, i.Entity.ZipCode, i.Entity.TotalRevenueBand, i.Entity.TotalEmployeesBand, i.Entity.RevenuePerCapitaBand, i.Entity.AverageRevenueBand, Place = i.Entity.Place })
                .Select(i => new DistanceEntity<Wrapper>
                {
                    Distance = i.Distance,
                    Entity = new Wrapper
                    {
                        Place = i.Place,
                        ZipCode = new Models.ZipCode
                        {
                            Id = i.ZipCode.Id,
                            Name = i.ZipCode.Name,
                            Zip = i.ZipCode.Zip
                        },
                        Centroid = new Geo.LatLng
                        {
                            Lat = i.Geography.CenterLat.Value,
                            Lng = i.Geography.CenterLong.Value
                        },
                        BoundingBox = new Geo.BoundingBox
                        {
                            NorthEast = new Geo.LatLng
                            {
                                Lat = i.Geography.North,
                                Lng = i.Geography.East
                            },
                            SouthWest = new Geo.LatLng
                            {
                                Lat = i.Geography.South,
                                Lng = i.Geography.West
                            }
                        },
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

                        TotalEmployeesBand = i.TotalEmployeesBand,
                        AverageRevenueBand = i.AverageRevenueBand,
                        TotalRevenueBand = i.TotalRevenueBand,
                        RevenuePerCapitaBand = i.RevenuePerCapitaBand
                    }
                });

            data = FilterQuery(data, filters);

            IQueryable<Models.Advertising> output = new List<Models.Advertising>().AsQueryable();
            output = data.Select(i => new Models.Advertising
            {
                PlaceId = i.Entity.Place.Id,
                StateSEOKey = i.Entity.Place.County.State.SEOKey,
                CountySEOKey = i.Entity.Place.County.SEOKey,
                CitySEOKey = i.Entity.Place.City.SEOKey,
                Centroid = i.Entity.Centroid,
                BoundingBox = i.Entity.BoundingBox,
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
