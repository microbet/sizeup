using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;


namespace SizeUp.Core.DataLayer.Base
{
    public class City
    {
        public static IQueryable<Data.City> In(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            IQueryable<Data.City> output = context.Cities.Where(i => 0 == 1);//creates empty set
            if (boundingGranularity == Granularity.County)
            {
                output = context.Cities.Where(i => i.CityCountyMappings.Any(p => p.County.CityCountyMappings.Any(m=> m.Id == placeId)));
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                output = context.Cities.Where(i => i.CityCountyMappings.Any(p => p.County.Metro.Counties.Any(co=> co.CityCountyMappings.Any(m=> m.Id == placeId) )));
            }
            else if (boundingGranularity == Granularity.State)
            {
                output = context.Cities.Where(i => i.CityCountyMappings.Any(p => p.County.State.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                output = context.Cities;
            }
            return output;
        }

    }
}
