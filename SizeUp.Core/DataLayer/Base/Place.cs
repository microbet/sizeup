using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer.Base
{
    public class Place : Base
    {
        public static IQueryable<Data.CityCountyMapping> Get(SizeUpContext context)
        {
            var data = context.CityCountyMappings
                       .Where(d => d.City.CityType.IsActive);
            return data;
        }

        public static IQueryable<Data.PlaceKeyword> Keywords(SizeUpContext context)
        {
            var data = context.PlaceKeywords
                       .Where(d => d.CityCountyMapping.City.CityType.IsActive);
            return data;
        }
    }
}
