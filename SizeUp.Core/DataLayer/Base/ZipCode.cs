using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;
using System.Data.Spatial;

namespace SizeUp.Core.DataLayer.Base
{
    public class ZipCode
    {
        public static IQueryable<Data.ZipCode> In(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            IQueryable<Data.ZipCode> output = context.ZipCodes.Where(i=>1==0);//creates empty set
            if (boundingGranularity == Granularity.City)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCityMappings.Any(z => z.City.CityCountyMappings.Any(m => m.Id == placeId)));
            }
            else if (boundingGranularity == Granularity.County)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.CityCountyMappings.Any(m => m.Id == placeId)));
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.Metro.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId)))); 
            }
            else if (boundingGranularity == Granularity.State)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.State.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                output = context.ZipCodes;
            }
            return output;
        }


        public static IQueryable<Models.Base.DistanceEntity<Data.ZipCode>> Distance(SizeUpContext context, LatLng latLng)
        {
            var data = context.ZipCodeGeographies.Where(i => i.GeographyClass.Name == Core.Geo.GeographyClass.Calculation)
                .Select(i => new
                {
                    Entity = i.ZipCode,
                    Geography = i
                })
                .Where(i=>i.Geography != null)
                .Select(i => new Models.Base.DistanceEntity<Data.ZipCode>
                {
                    Distance = System.Math.Pow(System.Math.Pow(((double)i.Geography.Geography.CenterLat - latLng.Lat) * 69.1, 2) + System.Math.Pow(((double)i.Geography.Geography.CenterLong - latLng.Lng) * (double)(System.Data.Objects.SqlClient.SqlFunctions.Cos(latLng.Lat / 57.3) * 69.1), 2), 0.5),
                    Entity = i.Entity
                });
            return data;
        }
    }
}
