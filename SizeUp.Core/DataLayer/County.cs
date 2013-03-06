using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer
{
    public class County
    {
        public static IQueryable<Data.County> In(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            IQueryable<Data.County> output = context.Counties.Where(i=>0==1);// creates empty set
            if (boundingGranularity == Granularity.Metro)
            {
                output= context.Counties.Where(i => i.CityCountyMappings.Any(p => p.County.Metro.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))));
            }
            else if (boundingGranularity == Granularity.State)
            {
                output = context.Counties.Where(i => i.CityCountyMappings.Any(p => p.County.State.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                output = context.Counties;
            }
            return output;
        }

    }
}
