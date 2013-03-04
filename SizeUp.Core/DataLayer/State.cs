using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer
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
    }
}
