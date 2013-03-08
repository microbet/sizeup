using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer.Base
{
    public class ZipCode
    {
        public static IQueryable<Data.ZipCode> In(SizeUpContext context, long placeId, Granularity boundingGranularity)
        {
            IQueryable<Data.ZipCode> output = context.ZipCodes.Where(i=>1==0);//creates empty set
            if (boundingGranularity == Granularity.City)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCityMappings.Any(z => z.City.CityCountyMappings.Any(m => m.Id == placeId)));
            }
            else if (boundingGranularity == Granularity.County)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.CityCountyMappings.Any(m => m.Id == placeId)));
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.Metro.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId)))); 
            }
            else if (boundingGranularity == Granularity.State)
            {
                output = context.ZipCodes.Where(i => i.ZipCodeCountyMappings.Any(z => z.County.State.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                output = context.ZipCodes;
            }
            return output;
        }
    }
}
