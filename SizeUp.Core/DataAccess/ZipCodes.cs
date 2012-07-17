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

    }
}
