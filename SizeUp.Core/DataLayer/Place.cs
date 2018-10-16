using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.SqlClient;
namespace SizeUp.Core.DataLayer
{
    public class Place
    {
        public static IQueryable<Data.Place> Get(SizeUpContext context)
        {
            return context.Places.Where(i => i.City.CityType.IsActive && i.City.IsActive);
        }


        public static Models.Place Get(SizeUpContext context, long? id)
        {
            return Get(context)
                .Where(i => i.Id == id)
                .Select(new Projections.Place.Default().Expression)
                .FirstOrDefault();
        }

        public static List<Models.Place> Get(SizeUpContext context, List<long> id)
        {
            return Get(context)
                .Where(i => id.Contains(i.Id))
                .Select(new Projections.Place.Default().Expression)
                .ToList();
        }

        public static Models.Place GetByBusiness(SizeUpContext context, long businessId)
        {
            return Get(context)
                .Where(i => i.City.Businesses.Any(bc => bc.Id == businessId))
                .Select(new Projections.Place.Default().Expression)
                .FirstOrDefault();
        }

        public static Models.Place GetLegacy(SizeUpContext context, string SEOKey)
        {
            return Get(context)
                .Where(i => i.City.LegacyCommunitySEOKeys.Any(l => l.SEOKey == SEOKey))
                .Select(new Projections.Place.Default().Expression)
                .FirstOrDefault();
        }

        /**
         * For some reason, the Get routine wants to return this object instead of a null.
         * Instead of researching why, I made the object a global sentinel so that, although
         * I can't check for null, I can check whether the return value is NOT_FOUND.
         */
        public static Models.Place NOT_FOUND = new Models.Place() { City = new Models.City(), County = new Models.County(), Metro = new Models.Metro(), State = new Models.State() };

        public static Models.Place Get(SizeUpContext context, string stateSEOKey, string countySEOKey, string citySEOKey, string metroSEOKey = null)
        {
            Models.Place output = null;
            if (!string.IsNullOrEmpty(stateSEOKey) && !string.IsNullOrEmpty(countySEOKey) && !string.IsNullOrEmpty(citySEOKey))
            {
                output = Get(context)
                    .Where(i => i.County.SEOKey == countySEOKey && i.County.State.SEOKey == stateSEOKey && i.City.SEOKey == citySEOKey)
                    .Select(new Projections.Place.Default().Expression)
                    .FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(stateSEOKey) && !string.IsNullOrEmpty(countySEOKey))
            {
                output = Get(context)
                    .Where(i => i.County.SEOKey == countySEOKey && i.County.State.SEOKey == stateSEOKey)
                    .Select(new Projections.Place.County().Expression)
                    .FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(stateSEOKey))
            {
                output = Get(context)
                   .Where(i => i.County.State.SEOKey == stateSEOKey)
                   .Select(new Projections.Place.State().Expression)
                   .FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(metroSEOKey))
            {
                output = Get(context)
                    .Where(i => i.County.Metro.SEOKey == metroSEOKey)
                    .Select(new Projections.Place.Metro().Expression)
                    .FirstOrDefault();
            }
            return output != null ? output : NOT_FOUND;
        }

        public static IQueryable<Models.DistanceEntity<Models.Place>> ListNear(SizeUpContext context, Core.Geo.LatLng latLng)
        {
            var distanceFilter = new DistanceEntity<Data.Place>.DistanceEntityFilter(latLng);
            return Get(context)
                .Select(i => new KeyValue<Data.Place, Geo.LatLng>
                {
                    Key = i,
                    Value = i.GeographicLocation.Geographies.AsQueryable().Where(g => g.GeographyClass.Name == Geo.GeographyClass.Calculation)
                    .Select(g => new Geo.LatLng { Lat = g.CenterLat.Value, Lng = g.CenterLong.Value })
                    .FirstOrDefault()
                })
                .Select(distanceFilter.Projection)
                .Select(new Projections.Place.Distance().Expression);
        }

        public static Models.Place GetBoundingCity(SizeUpContext context, Core.Geo.LatLng latLng)
        {
            var distanceFilter = new DistanceEntity<Data.Place>.DistanceEntityFilter(latLng);
            var locations = Core.DataLayer.Geography.Get(context).Where(x =>
                    x.West <= latLng.Lng && x.East >= latLng.Lng
                    && x.North >= latLng.Lat && x.South <= latLng.Lat
                );

            foreach (var location in locations)
            {
                var geoId = location.GeographicLocationId;
                var geographicLocation = Core.DataLayer.GeographicLocation.Get(context).Any(x => x.Id == geoId && x.GranularityId == 4);

                if (geographicLocation == true)
                {
                    return Core.DataLayer.Place.Get(context).Where(x => x.GeographicLocation.Id == geoId)
                        .Select(new Projections.Place.Default().Expression)
                        .FirstOrDefault();                    
                }
            }
            return null;
        }

        public static List<Models.Place> List(SizeUpContext context, List<long> placeIds)
        {
            return Get(context)
                .Where(i => placeIds.Contains(i.Id))
                .Select(new Projections.Place.Default().Expression)
                .ToList();
        }

        public static IQueryable<Models.Place> List(SizeUpContext context)
        {
            return Get(context)
                .Select(new Projections.Place.Default().Expression);
        }

        public class SearchResult
        {
            public Data.Place Place;
            public String Search;
        }

        public static string nationQuery = @"
            SELECT TOP(@MaxResults)
                Place.Id as id,
                GeographicLocation.LongName as displayName, -- also LongName, ShortName
                City.Id as city_Id,
                City.Name as city_Name, -- also ShortName
                City.SEOKey as city_SEOKey,
                CityType.Name as city_TypeName,
                County.Id as county_Id,
                County.Name as county_Name, -- also ShortName
                County.SEOKey as county_SEOKey,
                Metro.Id as metro_Id,
                Metro.Name as metro_Name, -- also LongName, ShortName
                Metro.SEOKey as metro_SEOKey,
                State.Id as state_Id,
                State.Name as state_Name, -- also LongName
                State.Abbreviation as state_Abbreviation, -- also ShortName
                State.SEOKey as state_SEOKey
            FROM Place
                inner join GeographicLocation on (place.Id = GeographicLocation.Id)
                inner join City on (place.CityId = City.Id)
                inner join CityType on (City.CityTypeId = CityType.Id)
                inner join County on (place.CountyId = County.Id)
                inner join Metro on (county.MetroId = Metro.Id)
                inner join State on (county.StateId = State.Id)
                inner join Demographics on (Demographics.GeographicLocationId = City.Id)
            WHERE CityType.IsActive = 1
                AND City.IsActive = 1
                AND City.Name LIKE @Term + '%'
                AND Demographics.Year = @Year
                AND Demographics.Quarter = @Quarter
            ORDER BY Demographics.TotalPopulation DESC
        ";

        public static string countyQuery = @"
            SELECT TOP(@MaxResults)
                Place.Id as id,
                GeographicLocation.LongName as displayName, -- also LongName, ShortName
                City.Id as city_Id,
                City.Name as city_Name, -- also ShortName
                City.SEOKey as city_SEOKey,
                CityType.Name as city_TypeName,
                County.Id as county_Id,
                County.Name as county_Name, -- also ShortName
                County.SEOKey as county_SEOKey,
                Metro.Id as metro_Id,
                Metro.Name as metro_Name, -- also LongName, ShortName
                Metro.SEOKey as metro_SEOKey,
                State.Id as state_Id,
                State.Name as state_Name, -- also LongName
                State.Abbreviation as state_Abbreviation, -- also ShortName
                State.SEOKey as state_SEOKey
            FROM Place
                inner join GeographicLocation on (place.Id = GeographicLocation.Id)
                inner join City on (place.CityId = City.Id)
                inner join CityType on (City.CityTypeId = CityType.Id)
                inner join County on (place.CountyId = County.Id)
                inner join Metro on (county.MetroId = Metro.Id)
                inner join State on (county.StateId = State.Id)
                inner join Demographics on (Demographics.GeographicLocationId = City.Id)
            WHERE CityType.IsActive = 1
                AND City.IsActive = 1
                AND City.Name LIKE '%' + @Term + '%'
                AND Demographics.Year = @Year
                AND Demographics.Quarter = @Quarter
                AND County.Id in (select Item from @countyIds)
            ORDER BY Demographics.TotalPopulation DESC
        ";

        public static Models.Place populateFromQuery(SqlDataReader r)
        {
            return new Models.Place
            {
                Id = r.GetInt64(0),
                DisplayName = r.GetString(1),
                LongName = r.GetString(1),
                ShortName = r.GetString(1),
                City = new Models.City()
                {
                    Id = r.GetInt64(2),
                    Name = r.GetString(3),
                    SEOKey = r.GetString(4),
                    TypeName = r.GetString(5),
                    LongName = r.GetString(3) + ", " + r.GetString(14),
                    ShortName = r.GetString(3)
                },
                County = new Models.County
                {
                    Id = r.GetInt64(6),
                    Name = r.GetString(7),
                    SEOKey = r.GetString(8),
                    LongName = r.GetString(7) + ", " + r.GetString(14),
                    ShortName = r.GetString(7)
                },
                Metro = new Models.Metro
                {
                    Id = r.GetInt64(9),
                    Name = r.GetString(10),
                    SEOKey = r.GetString(11),
                    LongName = r.GetString(10),
                    ShortName = r.GetString(10)
                },
                State = new Models.State
                {
                    Id = r.GetInt64(12),
                    Name = r.GetString(13),
                    Abbreviation = r.GetString(14),
                    SEOKey = r.GetString(15),
                    LongName = r.GetString(13),
                    ShortName = r.GetString(14)
                }
            };
        }

        public static IQueryable<Models.Place> LocalSearch(SizeUpContext context, string term, long[] countyIds, int maxResults)
        {
            // TODO: cap maxResults at ... 200?
            // TODO: re-incorporate PlaceKeywords, then replace old Search() with this one
            var conn = new SqlConnection(((System.Data.EntityClient.EntityConnection)context.Connection).StoreConnection.ConnectionString);
            using (conn)
            {
                conn.Open();
                SqlCommand cmd;
                if (countyIds != null && countyIds.Length != 0) {
                    cmd = new SqlCommand(countyQuery, conn);
                } else {
                    cmd = new SqlCommand(nationQuery, conn);
                }

                List<Models.Place> results = new List<Models.Place>();
                using (cmd)
                {
                    cmd.Parameters.Add(new SqlParameter("@Term", term));
                    cmd.Parameters.Add(new SqlParameter("@MaxResults", maxResults));
                    cmd.Parameters.Add(new SqlParameter("@Year", CommonFilters.TimeSlice.Demographics.Year));
                    cmd.Parameters.Add(new SqlParameter("@Quarter", CommonFilters.TimeSlice.Demographics.Quarter));
                    using (System.Data.DataTable table = new System.Data.DataTable())
                    {
                        if (countyIds != null && countyIds.Length != 0)
                        {
                            table.Columns.Add("Item", typeof(long));
                            for (int i = 0; i < countyIds.Length; i++)
                            {
                                table.Rows.Add(countyIds[i]);
                            }
                            var countyIdsParam = new SqlParameter("@countyIds", System.Data.SqlDbType.Structured);
                            countyIdsParam.TypeName = "dbo.IdList";
                            countyIdsParam.Value = table;
                            cmd.Parameters.Add(countyIdsParam);
                        }
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(populateFromQuery(reader));
                            }
                        }
                    }
                }

                return new EnumerableQuery<Models.Place>(results);
            }
        }

        public static IQueryable<Models.Place> Search(SizeUpContext context, string term, long[] countyIds)
        {
            var searchResults =
                Get(context)
                .Select(i => new SearchResult { Place = i, Search = i.City.Name })
                .Union(
                    Get(context)
                    .SelectMany(i => i.PlaceKeywords)
                    .Select(i => new SearchResult { Place = i.Place, Search = i.Name })
                );

            // Filter by search term
            if ( ! string.Empty.Equals(term))
            {
                var qs = term.Split(',').ToList();
                string city = qs[0].Trim();
                searchResults = searchResults.Where(i => i.Search.StartsWith(city));
                if (qs.Count > 1)
                {
                    string state = qs[1].Trim();
                    searchResults = searchResults.Where(i => i.Place.County.State.Abbreviation.StartsWith(state));
                }
            }

            // Filter by county IDs (TODO: generalize to geographic location IDs)
            if (countyIds != null && countyIds.Length != 0)
            {
                searchResults = searchResults.Where(i => countyIds.Contains(i.Place.County.Id));
            }

            return searchResults
                .OrderByDescending(i => i.Place.City.GeographicLocation.Demographics.AsQueryable().Where(d => d.Year == CommonFilters.TimeSlice.Demographics.Year && d.Quarter == CommonFilters.TimeSlice.Demographics.Quarter).FirstOrDefault().TotalPopulation)
                .Select(i => i.Place)
                .Select(new Projections.Place.Default().Expression);
        }
    }
}
