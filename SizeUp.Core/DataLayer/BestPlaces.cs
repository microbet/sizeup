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
    public class BestPlaces
    {
        protected class Wrapper
        {
            public Data.GeographicLocation Place { get; set; }
            public Data.Industry Industry { get; set; }
            public Core.Geo.LatLng Centroid { get; set; }
            public Core.Geo.BoundingBox BoundingBox { get; set; }
            public long? AverageRevenue { get; set; }
            public long? TotalRevenue { get; set; }
            public long? TotalEmployees { get; set; }
            public long? AverageEmployees { get; set; }
            public double? EmployeesPerCapita { get; set; }
            public long? RevenuePerCapita { get; set; }
            public long? HouseholdIncome { get; set; }
            public long? Population { get; set; }
            public long? AirportsNearby { get; set; }
            public long? UniversitiesNearby { get; set; }
            public double? BachelorsDegreeOrHigher { get; set; }
            public double? HighSchoolOrHigher { get; set; }
            public double? WhiteCollarWorkers { get; set; }
            public double? MedianAge { get; set; }
            public double? HouseholdExpenditures { get; set; }
            public double? BlueCollarWorkers { get; set; }
            public double? YoungEducated { get; set; }
            public double? CommuteTime { get; set; }

            public Band<double> AverageRevenueBand { get; set; }
            public Band<double> TotalRevenueBand { get; set; }
            public Band<double> TotalEmployeesBand { get; set; }
            public Band<double> AverageEmployeesBand { get; set; }
            public Band<double> RevenuePerCapitaBand { get; set; }
            public Band<double> EmployeesPerCapitaBand { get; set; }
        }

        private static readonly long POPULATION_MIN = 100000;
        protected static IQueryable<Wrapper> FilterQuery(IQueryable<Wrapper> data, BestPlacesFilters filters)
        {
            if (filters.AverageRevenue != null)
            {
                if (filters.AverageRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.AverageRevenue >= filters.AverageRevenue.Min);
                }
                if (filters.AverageRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.AverageRevenue <= filters.AverageRevenue.Max);
                }
            }

            if (filters.TotalRevenue != null)
            {
                if (filters.TotalRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.TotalRevenue >= filters.TotalRevenue.Min);
                }
                if (filters.TotalRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.TotalRevenue <= filters.TotalRevenue.Max);
                }
            }

            if (filters.TotalEmployees != null)
            {
                if (filters.TotalEmployees.Min.HasValue)
                {
                    data = data.Where(i => i.TotalEmployees >= filters.TotalEmployees.Min);
                }
                if (filters.TotalEmployees.Max.HasValue)
                {
                    data = data.Where(i => i.TotalEmployees <= filters.TotalEmployees.Max);
                }
            }

            if (filters.AverageEmployees != null)
            {
                if (filters.AverageEmployees.Min.HasValue)
                {
                    data = data.Where(i => i.AverageEmployees >= filters.AverageEmployees.Min);
                }
                if (filters.TotalEmployees.Max.HasValue)
                {
                    data = data.Where(i => i.AverageEmployees <= filters.AverageEmployees.Max);
                }
            }

            if (filters.RevenuePerCapita != null)
            {
                if (filters.RevenuePerCapita.Min.HasValue)
                {
                    data = data.Where(i => i.RevenuePerCapita >= filters.RevenuePerCapita.Min);
                }
                if (filters.RevenuePerCapita.Max.HasValue)
                {
                    data = data.Where(i => i.RevenuePerCapita <= filters.RevenuePerCapita.Max);
                }
            }

            if (filters.HouseholdIncome != null)
            {
                if (filters.HouseholdIncome.Min.HasValue)
                {
                    data = data.Where(i => i.HouseholdIncome >= filters.HouseholdIncome.Min);
                }
                if (filters.HouseholdIncome.Max.HasValue)
                {
                    data = data.Where(i => i.HouseholdIncome <= filters.HouseholdIncome.Max);
                }
            }

            if (filters.HouseholdExpenditures != null)
            {
                if (filters.HouseholdExpenditures.Min.HasValue)
                {
                    data = data.Where(i => i.HouseholdExpenditures >= filters.HouseholdExpenditures.Min);
                }
                if (filters.HouseholdExpenditures.Max.HasValue)
                {
                    data = data.Where(i => i.HouseholdExpenditures <= filters.HouseholdExpenditures.Max);
                }
            }

            if (filters.MedianAge != null)
            {
                if (filters.MedianAge.Min.HasValue)
                {
                    data = data.Where(i => i.MedianAge >= filters.MedianAge.Min);
                }
                if (filters.MedianAge.Max.HasValue)
                {
                    data = data.Where(i => i.MedianAge <= filters.MedianAge.Max);
                }
            }

            if (filters.BachelorOrHigher != null)
            {
                var v = filters.BachelorOrHigher / 100.0d;
                data = data.Where(i => i.BachelorsDegreeOrHigher >= v);
            }

            if (filters.HighSchoolOrHigher != null)
            {
                var v = filters.HighSchoolOrHigher / 100.0d;
                data = data.Where(i => i.HighSchoolOrHigher >= v);
            }

            if (filters.WhiteCollarWorkers != null)
            {
                var v = filters.WhiteCollarWorkers / 100.0d;
                data = data.Where(i => i.WhiteCollarWorkers >= v);
            }

            if (filters.BlueCollarWorkers != null)
            {
                var v = filters.BlueCollarWorkers / 100.0d;
                data = data.Where(i => i.BlueCollarWorkers >= v);
            }

            if (filters.YoungEducated != null)
            {
                var v = filters.YoungEducated / 100.0d;
                data = data.Where(i => i.YoungEducated >= v);
            }

            if (filters.AirportsNearby != null)
            {
                data = data.Where(i => i.AirportsNearby >= filters.AirportsNearby);
            }

            if (filters.UniversitiesNearby != null)
            {
                data = data.Where(i => i.UniversitiesNearby >= filters.UniversitiesNearby);
            }

            if (filters.CommuteTime != null)
            {
                data = data.Where(i => i.CommuteTime <= filters.CommuteTime);
            }


            if (filters.Attribute == "totalRevenue")
            {
                data = data.Where(i => i.TotalRevenue != null && i.TotalRevenue > 0);
            }
            else if (filters.Attribute == "averageRevenue")
            {
                data = data.Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);
            }
            else if (filters.Attribute == "revenuePerCapita")
            {
                data = data.Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "underservedMarkets")
            {
                data = data.Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "employeesPerCapita")
            {
                data = data.Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);
            }
            else if (filters.Attribute == "householdIncome")
            {
                data = data.Where(i => i.HouseholdIncome != null && i.HouseholdIncome > 0);
            }
            else if (filters.Attribute == "whiteCollarWorkers")
            {
                data = data.Where(i => i.WhiteCollarWorkers != null && i.WhiteCollarWorkers > 0);
            }
            else if (filters.Attribute == "totalEmployees")
            {
                data = data.Where(i => i.TotalEmployees != null && i.TotalEmployees > 0);
            }
            else if (filters.Attribute == "averageEmployees")
            {
                data = data.Where(i => i.AverageEmployees != null && i.AverageEmployees > 0);
            }
            else if (filters.Attribute == "householdExpenditures")
            {
                data = data.Where(i => i.HouseholdExpenditures != null && i.HouseholdExpenditures > 0);
            }
            else if (filters.Attribute == "medianAge")
            {
                data = data.Where(i => i.MedianAge != null && i.MedianAge > 0);
            }
            else if (filters.Attribute == "bachelorsDegreeOrHigher")
            {
                data = data.Where(i => i.BachelorsDegreeOrHigher != null && i.BachelorsDegreeOrHigher > 0);
            }
            else if (filters.Attribute == "highSchoolOrHigher")
            {
                data = data.Where(i => i.HighSchoolOrHigher != null && i.HighSchoolOrHigher > 0);
            }
            else if (filters.Attribute == "blueCollarWorkers")
            {
                data = data.Where(i => i.BlueCollarWorkers != null && i.BlueCollarWorkers > 0);
            }
            else if (filters.Attribute == "airportsNearby")
            {
                data = data.Where(i => i.AirportsNearby != null && i.AirportsNearby > 0);
            }
            else if (filters.Attribute == "youngEducated")
            {
                data = data.Where(i => i.YoungEducated != null && i.YoungEducated > 0);
            }
            else if (filters.Attribute == "universitiesNearby")
            {
                data = data.Where(i => i.UniversitiesNearby != null && i.UniversitiesNearby > 0);
            }
            else if (filters.Attribute == "commuteTime")
            {
                data = data.Where(i => i.CommuteTime != null && i.CommuteTime > 0);
            }

            data = data.Where(i => i.Population >= POPULATION_MIN);

            switch (filters.Attribute)
            {
                case "totalRevenue":
                    data = data.OrderByDescending(i => i.TotalRevenue);
                    break;
                case "averageRevenue":
                    data = data.OrderByDescending(i => i.AverageRevenue);
                    break;
                case "revenuePerCapita":
                    data = data.OrderByDescending(i => i.RevenuePerCapita);
                    break;
                case "underservedMarkets":
                    data = data.OrderBy(i => i.RevenuePerCapita);
                    break;
                case "employeesPerCapita":
                    data = data.OrderByDescending(i => i.EmployeesPerCapita);
                    break;
                case "householdIncome":
                    data = data.OrderByDescending(i => i.HouseholdIncome);
                    break;
                case "whiteCollarWorkers":
                    data = data.OrderByDescending(i => i.WhiteCollarWorkers);
                    break;
                case "totalEmployees":
                    data = data.OrderByDescending(i => i.TotalEmployees);
                    break;
                case "averageEmployees":
                    data = data.OrderByDescending(i => i.AverageEmployees);
                    break;
                case "householdExpenditures":
                    data = data.OrderByDescending(i => i.HouseholdExpenditures);
                    break;
                case "medianAge":
                    data = data.OrderByDescending(i => i.MedianAge);
                    break;
                case "bachelorsDegreeOrHigher":
                    data = data.OrderByDescending(i => i.BachelorsDegreeOrHigher);
                    break;
                case "highSchoolOrHigher":
                    data = data.OrderByDescending(i => i.HighSchoolOrHigher);
                    break;
                case "blueCollarWorkers":
                    data = data.OrderByDescending(i => i.BlueCollarWorkers);
                    break;
                case "airportsNearby":
                    data = data.OrderByDescending(i => i.AirportsNearby);
                    break;
                case "youngEducated":
                    data = data.OrderByDescending(i => i.YoungEducated);
                    break;
                case "universitiesNearby":
                    data = data.OrderByDescending(i => i.UniversitiesNearby);
                    break;
                case "commuteTime":
                    data = data.OrderByDescending(i => i.CommuteTime);
                    break;
            }
            return data;
        }


        public static IQueryable<Models.BestPlaces> Get(SizeUpContext context, long industryId, long? regionId, long? stateId, BestPlacesFilters filters, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = context.GeographicLocations
                .SelectMany(i => i.Demographics, (i, o) => new { Place = i, Demographics = o })
                .SelectMany(i => i.Place.IndustryDatas, (i, o) => new { i.Place, i.Demographics, IndustryData = o, Industry = o.Industry })
                .SelectMany(i => i.Place.Geographies, (i, o) => new { i.Place, i.IndustryData, i.Demographics, i.Industry, Geography = o })
                  .Where(i => i.IndustryData.Year == CommonFilters.TimeSlice.Industry.Year && i.IndustryData.Quarter == CommonFilters.TimeSlice.Industry.Quarter)
                    .Where(i => i.Demographics.Year == CommonFilters.TimeSlice.Demographics.Year && i.Demographics.Quarter == CommonFilters.TimeSlice.Demographics.Quarter)
                    .Where(i => i.Geography.GeographyClass.Name == Geo.GeographyClass.Calculation)
                    .Where(i => i.IndustryData.IndustryId == industryId)
                    .Where(i => i.Place.Granularity.Name == gran)
                    .Select(i => new Wrapper
                    {
                        Place = i.Place,
                        Industry = i.Industry,
                        AirportsNearby = i.Demographics.AirPortsWithinHalfMile,
                        BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                        BlueCollarWorkers = i.Demographics.BlueCollarWorkersPercentage,
                        CommuteTime = i.Demographics.CommuteTime,
                        HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                        HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,
                        HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                        MedianAge = i.Demographics.MedianAge,
                        Population = i.Demographics.TotalPopulation,
                        UniversitiesNearby = i.Demographics.UniversitiesWithinHalfMile,
                        WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                        YoungEducated = i.Demographics.YoungEducatedPercentage,
                        AverageEmployees = i.IndustryData.AverageEmployees,
                        AverageRevenue = i.IndustryData.AverageRevenue,
                        RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                        TotalEmployees = i.IndustryData.TotalEmployees,
                        TotalRevenue = i.IndustryData.TotalRevenue,
                        EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,
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
                        AverageEmployeesBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageEmployees).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        TotalEmployeesBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        EmployeesPerCapitaBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.Bands.Where(b => b.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault()
                    });


            if (regionId != null)
            {
                data = data.Where(i => i.Place.IntersectedGeographicLocations.Any(global => global.Id == regionId));
            }
            if (stateId != null)
            {
                data = data.Where(i => i.Place.IntersectedGeographicLocations.Any(global => global.Id == stateId));
            }
            data = FilterQuery(data, filters);

            IQueryable<Models.BestPlaces> output = new List<Models.BestPlaces>().AsQueryable();
            if (granularity == Granularity.City)
            {
                output = data.Select(i => new Models.BestPlaces
                {
                    State = new Models.State
                    {
                        Id = i.Place.City.State.Id,
                        Abbreviation = i.Place.City.State.Abbreviation,
                        Name = i.Place.City.State.Name,
                        SEOKey = i.Place.City.State.SEOKey,
                        LongName = i.Place.City.State.GeographicLocation.LongName,
                        ShortName = i.Place.City.State.GeographicLocation.ShortName
                    },
                    County = i.Place.City.Counties.Select(co=>new Models.County
                    {
                        Id = co.Id,
                        Name = co.Name,
                        SEOKey = co.SEOKey,
                        LongName = co.GeographicLocation.LongName,
                        ShortName = co.GeographicLocation.ShortName
                    }).FirstOrDefault(),
                    City = new Models.City
                    {
                        Id = i.Place.City.Id,
                        Name = i.Place.City.Name,
                        SEOKey = i.Place.City.SEOKey,
                        TypeName = i.Place.City.CityType.Name,
                        ShortName = i.Place.City.GeographicLocation.ShortName,
                        LongName = i.Place.City.GeographicLocation.LongName
                    },

                    Centroid = i.Centroid,
                    BoundingBox = i.BoundingBox,
                    TotalEmployees = i.TotalEmployeesBand,
                    AverageEmployees = i.AverageEmployeesBand,
                    EmployeesPerCapita = i.EmployeesPerCapitaBand,
                    TotalRevenue = i.TotalRevenueBand,
                    AverageRevenue = i.AverageRevenueBand,
                    RevenuePerCapita = i.RevenuePerCapitaBand
                });
            }
            else if (granularity == Granularity.County)
            {
                output = data.Select(i => new Models.BestPlaces
                {
                    State = new Models.State
                    {
                        Id = i.Place.County.State.Id,
                        Abbreviation = i.Place.County.State.Abbreviation,
                        Name = i.Place.County.State.Name,
                        SEOKey = i.Place.County.State.SEOKey,
                        LongName = i.Place.County.State.GeographicLocation.LongName,
                        ShortName = i.Place.County.State.GeographicLocation.ShortName
                    },
                    County = new Models.County
                    {
                        Id = i.Place.County.Id,
                        Name = i.Place.County.Name,
                        SEOKey = i.Place.County.SEOKey,
                        LongName = i.Place.County.GeographicLocation.LongName,
                        ShortName = i.Place.County.GeographicLocation.ShortName
                    },

                    Centroid = i.Centroid,
                    BoundingBox = i.BoundingBox,
                    TotalEmployees = i.TotalEmployeesBand,
                    AverageEmployees = i.AverageEmployeesBand,
                    EmployeesPerCapita = i.EmployeesPerCapitaBand,
                    TotalRevenue = i.TotalRevenueBand,
                    AverageRevenue = i.AverageRevenueBand,
                    RevenuePerCapita = i.RevenuePerCapitaBand
                });
            }
            else if (granularity == Granularity.Metro)
            {
                output = data.Select(i => new Models.BestPlaces
                {
                    Metro = new Models.Metro
                    {
                        Id = i.Place.Metro.Id,
                        Name = i.Place.Metro.Name,
                        SEOKey = i.Place.Metro.SEOKey,
                        LongName = i.Place.Metro.GeographicLocation.LongName,
                        ShortName = i.Place.Metro.GeographicLocation.ShortName
                    },
                    Centroid = i.Centroid,
                    BoundingBox = i.BoundingBox,
                    TotalEmployees = i.TotalEmployeesBand,
                    AverageEmployees = i.AverageEmployeesBand,
                    EmployeesPerCapita = i.EmployeesPerCapitaBand,
                    TotalRevenue = i.TotalRevenueBand,
                    AverageRevenue = i.AverageRevenueBand,
                    RevenuePerCapita = i.RevenuePerCapitaBand
                });
            }
            else if (granularity == Granularity.State)
            {
                output = data.Select(i => new Models.BestPlaces
                {
                    State = new Models.State
                    {
                        Id = i.Place.State.Id,
                        Abbreviation = i.Place.State.Abbreviation,
                        Name = i.Place.State.Name,
                        SEOKey = i.Place.State.SEOKey,
                        LongName = i.Place.State.GeographicLocation.LongName,
                        ShortName = i.Place.State.GeographicLocation.ShortName
                    },
                    Centroid = i.Centroid,
                    BoundingBox = i.BoundingBox,
                    TotalEmployees = i.TotalEmployeesBand,
                    AverageEmployees = i.AverageEmployeesBand,
                    EmployeesPerCapita = i.EmployeesPerCapitaBand,
                    TotalRevenue = i.TotalRevenueBand,
                    AverageRevenue = i.AverageRevenueBand,
                    RevenuePerCapita = i.RevenuePerCapitaBand
                });
            }
            return output;
        }

        public static List<Models.Band<double>> Bands(SizeUpContext context, long industryId, int itemCount, int bands, long? regionId, long? stateId, BestPlacesFilters filters, Granularity granularity)
        {
            var data = Get(context, industryId, regionId, stateId, filters, granularity)
                .Take(itemCount)
                .ToList();

            List<Models.Band<double>> output = new List<Models.Band<double>>();
            switch (filters.Attribute)
            {
                case "totalRevenue":
                    output = data.NTileDescending(i => i.TotalRevenue.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.TotalRevenue.Min), Max = b.Max(i => i.TotalRevenue.Max) })
                       .ToList();
                    break;
                case "averageRevenue":
                    output = data.NTileDescending(i => i.AverageRevenue.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.AverageRevenue.Min), Max = b.Max(i => i.AverageRevenue.Max) })
                       .ToList();
                    break;
                case "revenuePerCapita":
                    output = data.NTileDescending(i => i.RevenuePerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.RevenuePerCapita.Min), Max = b.Max(i => i.RevenuePerCapita.Max) })
                       .ToList();
                    break;
                case "underservedMarkets":
                    output = data.NTile(i => i.RevenuePerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.RevenuePerCapita.Min), Max = b.Max(i => i.RevenuePerCapita.Max) })
                       .ToList();
                    break;
                case "totalEmployees":
                    output = data.NTileDescending(i => i.TotalEmployees.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.TotalEmployees.Min), Max = b.Max(i => i.TotalEmployees.Max) })
                       .ToList();
                    break;
                case "averageEmployees":
                    output = data.NTileDescending(i => i.AverageEmployees.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.AverageEmployees.Min), Max = b.Max(i => i.AverageEmployees.Max) })
                       .ToList();
                    break;
                case "employeesPerCapita":
                    output = data.NTileDescending(i => i.EmployeesPerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.EmployeesPerCapita.Min), Max = b.Max(i => i.EmployeesPerCapita.Max) })
                       .ToList();
                    break;
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

        /*
        public static List<Models.KeyValue<int, Models.Industry>> IndustryRanks(SizeUpContext context, int rankCutoff, long placeId, Granularity granularity)
        {
            var place = Core.DataLayer.Place.List(context);
            var industryData = Core.DataLayer.Base.IndustryData.City(context);
            var demographics = Core.DataLayer.Base.DemographicsData.City(context);
            var industries = Core.DataLayer.Base.Industry.GetActive(context);

            var raw = industryData.Join(demographics, i => i.CityId, o => o.CityId, (i, o) => new { demographics = o, industryData = i })
                //.Where(i => i.demographics.TotalPopulation > 100000)
                        .Select(i => i.industryData)
                        .Where(ii => ii.TotalRevenue != null && ii.TotalRevenue > 0);
                        //.OrderByDescending(ii => ii.TotalRevenue);

            context.Industries.Select((i, index) => new { industry = i, num = index + 1 }).ToList();

           // var d = industries.Where(i => raw.Where(r => r.IndustryId == i.Id).Any(r => r.City.CityCountyMappings.Any(p => p.Id == placeId)));
           // d.ToList();

            return null;
        }
        */
        protected class Temp
        {
            public long Rank { get; set; }
            public long CityId { get; set; }
            public long IndustryId { get; set; }
        }
    }
}
