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
                .Where(i => i.City.Businesses.Any(bc=>bc.Id == businessId))
                .Select(new Projections.Place.Default().Expression)
                .FirstOrDefault();
        }

        public static Models.Place GetLegacy(SizeUpContext context, string SEOKey)
        {
            return Get(context)
                .Where(i => i.City.LegacyCommunitySEOKeys.Any(l=>l.SEOKey == SEOKey))
                .Select(new Projections.Place.Default().Expression)
                .FirstOrDefault();
        }

        
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
                    .Where(i => i.County.Metro.SEOKey == stateSEOKey)
                    .Select(new Projections.Place.Metro().Expression)
                    .FirstOrDefault();
            }
            return output != null ? output : new Models.Place() { City = new Models.City(), County = new Models.County(), Metro = new Models.Metro(), State = new Models.State() };
        }
        
        public static IQueryable<Models.DistanceEntity<Models.Place>> ListNear(SizeUpContext context, Core.Geo.LatLng latLng)
        {
            var distanceFilter = new DistanceEntity<Data.Place>.DistanceEntityFilter(latLng);
            return Get(context)
                .Select(i => new KeyValue<Data.Place, Geo.LatLng>
                {
                    Key = i,
                    Value = i.GeographicLocation.Geographies.AsQueryable().Where(g=>g.GeographyClass.Name == Geo.GeographyClass.Calculation)
                    .Select(g => new Geo.LatLng { Lat = g.CenterLat.Value, Lng = g.CenterLong.Value })
                    .FirstOrDefault()
                })
                .Select(distanceFilter.Projection)
                .Select(new Projections.Place.Distance().Expression);
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


        public static IQueryable<Models.Place> Search(SizeUpContext context, string term)
        {
            var qs = term.Split(',').ToList();
            string city = qs[0].Trim();
            string state = qs.Count > 1 ? qs[1].Trim() : string.Empty;
            var places = Get(context);
            return places
                    .Select(i => new
                    {
                        Place = i,
                        Search = i.City.Name
                    })
                    .Union(places
                    .SelectMany(i => i.PlaceKeywords)
                    .Select(i => new
                    {
                        Place = i.Place,
                        Search = i.Name
                    }))
                    .Where(i => i.Search.StartsWith(city))
                    .Where(i => i.Place.County.State.Abbreviation.StartsWith(state))
                    .OrderBy(i => i.Place.City.Name)
                    .ThenBy(i => i.Place.City.State.Abbreviation)
                    .ThenByDescending(i => i.Place.GeographicLocation.Demographics.AsQueryable().Where(d => d.Year == CommonFilters.TimeSlice.Demographics.Year && d.Quarter == CommonFilters.TimeSlice.Demographics.Quarter).FirstOrDefault().TotalPopulation)
                    .Select(i => i.Place)
                    .Select(new Projections.Place.Default().Expression);
        }
    }
}
