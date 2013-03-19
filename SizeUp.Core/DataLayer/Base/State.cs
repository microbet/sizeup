using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Base
{
    public class State
    {
        public static IQueryable<Data.State> In(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            IQueryable<Data.State> output = context.States.Where(i => 0 == 1);// creates empty set
            if (boundingGranularity == Granularity.Nation)
            {
                output = context.States;
            }
            return output;
        }

        public static IQueryable<Data.State> Get(SizeUpContext context)
        {
            return context.States;
        }
    }
}
