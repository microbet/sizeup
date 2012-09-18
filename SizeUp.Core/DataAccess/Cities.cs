using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess
{
    public static class Cities
    {
        public static IQueryable<Data.City> GetBounded(SizeUpContext context, BoundingEntity boundingEntity)
        {

            IQueryable<City> entity = context.Cities;
            if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
            {
                entity = context.CityCountyMappings
                    .Where(i => i.CountyId == boundingEntity.EntityId && i.City.CityType.IsActive)
                    .Select(i => i.City);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                entity = context.CityCountyMappings
                    .Where(i => i.County.MetroId == boundingEntity.EntityId && i.City.CityType.IsActive)
                    .Select(i => i.City);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                entity = context.CityCountyMappings
                    .Where(i => i.County.StateId == boundingEntity.EntityId && i.City.CityType.IsActive)
                    .Select(i => i.City);
            }
            return entity;
        }

        public static IQueryable<Models.EntityDistance<Data.City>> GetWithDistance(SizeUpContext context, double lat, double lng)
        {
            var scalar = 69.1 * System.Math.Cos(lat / 57.3);
            IQueryable<Models.EntityDistance<Data.City>> entity = context.CityGeographies
                .Where(i => i.GeographyClass.Name == "Calculation" && i.City.CityType.IsActive)
                .Select(i => new Models.EntityDistance<Data.City>()
                {
                    Distance = System.Math.Pow(System.Math.Pow(((double)i.Geography.CenterLat.Value - lat) * 69.1, 2) + System.Math.Pow(((double)i.Geography.CenterLong.Value - lng) * scalar, 2), 0.5),
                    Entity = i.City
                });
            return entity;
        }


    }
}
