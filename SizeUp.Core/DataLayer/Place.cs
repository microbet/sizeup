using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
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
