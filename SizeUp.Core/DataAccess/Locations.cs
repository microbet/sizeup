using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataAccess
{
    public static class Locations
    {

        public static IQueryable<Models.Locations> Get(SizeUpContext context, long placeId)
        {
            return context.CityCountyMappings
                   .Where(i => i.Id == placeId)
                   .Select(i => new Models.Locations()
                   {
                       City = i.City,
                       County = i.County,
                       Metro = i.County.Metro,
                       State = i.County.State
                   }); 
        }


        public static IQueryable<Models.Locations> Get(SizeUpContext context,  long cityId, long countyId)
        {
            return context.CityCountyMappings
                   .Select(i => new Models.Locations()
                   {
                       City = i.City,
                       County = i.County,
                       Metro = i.County.Metro,
                       State = i.County.State
                   })
                   .Where(i => i.County.Id == countyId && i.City.Id == cityId);
        }
        /*
        public static IQueryable<Models.Locations> Get(SizeUpContext context, long countyId)
        {
            return context.CityCountyMappings
                   .Select(i => new Models.Locations()
                   {
                       County = i.County,
                       Metro = i.County.Metro,
                       State = i.County.State
                   })
                   .Where(i => i.County.Id == countyId);
        }
         * */
    }
}
