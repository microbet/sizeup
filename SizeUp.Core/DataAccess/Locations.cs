using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataAccess
{
    public static class Locations
    {

        public static IQueryable<Models.Locations> Get(SizeUpContext context, long placeId)
        {
            return context.CityCountyMappings
                   .Where(i => i.Id == placeId && i.City.CityType.IsActive)
                   .Select(i => new Models.Locations()
                   {
                       Id = i.Id,
                       City = i.City,
                       County = i.County,
                       Metro = i.County.Metro,
                       State = i.County.State
                   }); 
        }


        public static IQueryable<Models.Locations> Get(SizeUpContext context,  long cityId, long countyId)
        {
            return context.CityCountyMappings
                   .Select(i => new Models.Locations()
                   {
                       Id = i.Id,
                       City = i.City,
                       County = i.County,
                       Metro = i.County.Metro,
                       State = i.County.State
                   })
                   .Where(i => i.County.Id == countyId && i.City.Id == cityId && i.City.CityType.IsActive);
        }

        public static IQueryable<Models.PlaceDistance> GetWithDistance(SizeUpContext context, double lat, double lng)
        {
            var scalar = 69.1 * System.Math.Cos(lat / 57.3);
            IQueryable<Models.PlaceDistance> entity = context.CityCountyMappings
                .Where(i=> i.City.CityType.IsActive)
                .Select(i=> new {
                    CityGeo = i.City.CityGeographies.Where(g=>g.GeographyClass.Name == "Calculation").FirstOrDefault().Geography,
                    CountyGeo = i.County.CountyGeographies.Where(g=>g.GeographyClass.Name == "Calculation").FirstOrDefault().Geography,
                    Location = new Models.Locations(){
                        Id = i.Id,
                        City = i.City,
                        County = i.County,
                        Metro = i.County.Metro,
                        State = i.County.State
                    }
                })
                .Select(i => new Models.PlaceDistance()
                {
                    CityDistance = System.Math.Pow(System.Math.Pow(((double)i.CityGeo.CenterLat.Value - lat) * 69.1, 2) + System.Math.Pow(((double)i.CityGeo.CenterLong.Value - lng) * scalar, 2), 0.5),
                    CountyDistance = System.Math.Pow(System.Math.Pow(((double)i.CountyGeo.CenterLat.Value - lat) * 69.1, 2) + System.Math.Pow(((double)i.CountyGeo.CenterLong.Value - lng) * scalar, 2), 0.5),
                    Entity = i.Location
                });
            return entity;
        }
    }
}
