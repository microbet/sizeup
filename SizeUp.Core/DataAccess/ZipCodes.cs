using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess
{
    public static class ZipCodes
    {
        public static IQueryable<Data.ZipCode> GetBounded(SizeUpContext context, BoundingEntity boundingEntity)
        {

            IQueryable<ZipCode> entity = context.ZipCodes;
            if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.City)
            {
                entity = context.ZipCodeCityMappings
                    .Where(i => i.CityId == boundingEntity.EntityId)
                    .Select(i => i.ZipCode);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.County)
            {
                entity = context.ZipCodeCountyMappings
                    .Where(i => i.CountyId == boundingEntity.EntityId)
                    .Select(i => i.ZipCode);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                entity = context.ZipCodeCountyMappings
                    .Where(i => i.County.MetroId == boundingEntity.EntityId)
                    .Select(i => i.ZipCode);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                entity = context.ZipCodeCountyMappings
                    .Where(i => i.County.StateId == boundingEntity.EntityId)
                    .Select(i => i.ZipCode);
            }


            return entity;
        }

        public static IQueryable<Data.ZipCode> GetWithin(SizeUpContext context, double lat, double lng, int miles)
        {
            var scalar = 69.1 * System.Math.Cos(lat / 57.3);
            IQueryable<ZipCode> entity = context.ZipCodeGeographies
                .Where(i => i.GeographyClass.Name == "Calculation")
                .Select(i => new
                {
                    Distance = System.Math.Pow(System.Math.Pow(((double)i.Geography.CenterLat.Value - lat) * 69.1, 2) + System.Math.Pow(((double)i.Geography.CenterLong.Value - lng) * scalar, 2), 0.5),
                    ZipCode = i.ZipCode
                })
                .Where(i => i.Distance <= (double)miles)
                .Select(i => i.ZipCode);
            return entity;
        }

    }
}
