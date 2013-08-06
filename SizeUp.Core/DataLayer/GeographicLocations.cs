using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.Spatial;
using SizeUp.Core.Geo;
using System.Data.Objects.SqlClient;


namespace SizeUp.Core.DataLayer
{
    public class GeographicLocation
    {
        public static IQueryable<Data.GeographicLocation> Get(SizeUpContext context)
        {
            return context.GeographicLocations;
        }

        public static IQueryable<Data.GeographicLocation> Get(SizeUpContext context, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);
            return Get(context).Where(i => i.Granularity.Name == gran);
        }

        public static IQueryable<Data.GeographicLocation> In(SizeUpContext context, Granularity granularity, long placeId, Granularity boundingGranularity)
        {
            var geos = Get(context, granularity);
            if (boundingGranularity == Core.DataLayer.Granularity.City)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.City.Places.Any(p => p.Id == placeId)));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.County)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.County.Places.Any(p => p.Id == placeId)));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Metro)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.Metro.Counties.Any(c => c.Places.Any(p => p.Id == placeId))));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.State)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.State.Counties.Any(c => c.Places.Any(p => p.Id == placeId))));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Nation)
            {
                //NOOP
            }
            return geos;
        }


        
    }
}
