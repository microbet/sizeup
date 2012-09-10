using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataAccess
{
    public static class States
    {
        public static IQueryable<Data.State> GetBounded(SizeUpContext context, BoundingEntity boundingEntity)
        {

            IQueryable<State> entity = context.States;
            return entity;
        }

    }
}
