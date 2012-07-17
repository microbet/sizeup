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
                    .Where(i => i.CountyId == boundingEntity.EntityId)
                    .Select(i => i.City);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                entity = context.CityCountyMappings
                    .Where(i => i.County.MetroId == boundingEntity.EntityId)
                    .Select(i => i.City);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                entity = context.CityCountyMappings
                    .Where(i => i.County.StateId == boundingEntity.EntityId)
                    .Select(i => i.City);
            }
            return entity;
        }

    }
}
