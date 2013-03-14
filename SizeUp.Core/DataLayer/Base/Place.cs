using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using System.Data.Spatial;

namespace SizeUp.Core.DataLayer.Base
{
    public class Place : Base
    {
        public static IQueryable<Data.CityCountyMapping> Get(SizeUpContext context)
        {
            var data = context.CityCountyMappings
                       .Where(d => d.City.CityType.IsActive);
            return data;
        }

        public static IQueryable<Models.Base.DistanceEntity<Data.CityCountyMapping>> Distance(SizeUpContext context, Core.Geo.LatLng latLng)
        {
            var scalar = 69.1 * System.Math.Cos(latLng.Lat / 57.3);
            var centroids = Core.DataLayer.Geography.Centroid(context, Granularity.Place);
            var data = Get(context).Join(centroids, i=>i.Id, o=>o.Key, (i,o)=> new { Centroid = o.Value, Place = i})
                       .Select(i=> new 
                       {
                           Entity = i.Place,
                           LatLng = i.Centroid
                       })
                       .Select(i => new Models.Base.DistanceEntity<Data.CityCountyMapping>
                       {
                           Distance = System.Math.Pow(System.Math.Pow(((double)i.LatLng.Lat - latLng.Lat) * 69.1, 2) + System.Math.Pow(((double)i.LatLng.Lng - latLng.Lng) * scalar, 2), 0.5),
                           Entity = i.Entity
                       });
            return data;
        }

        public static IQueryable<Data.PlaceKeyword> Keywords(SizeUpContext context)
        {
            var data = context.PlaceKeywords
                       .Where(d => d.CityCountyMapping.City.CityType.IsActive);
            return data;
        }
    }
}
