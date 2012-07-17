using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess
{
    public static class Counties
    {
        public static IQueryable<Data.County> GetBounded(SizeUpContext context, BoundingEntity boundingEntity)
        {

            IQueryable<County> entity = context.Counties;
            if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.Metro)
            {
                entity = context.Counties
                    .Where(i => i.MetroId == boundingEntity.EntityId);
            }
            else if (boundingEntity.EntityType == BoundingEntity.BoundingEntityType.State)
            {
                entity = context.Counties
                    .Where(i => i.StateId == boundingEntity.EntityId);
            }

            return entity;
        }

    }
}
