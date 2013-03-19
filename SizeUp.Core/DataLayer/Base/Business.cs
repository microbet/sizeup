using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataLayer.Base
{
    public class Business : Base
    {
        public static IQueryable<Data.Business> Get(SizeUpContext context)
        {
            var data = context.Businesses
                       .Where(d => d.IsActive);
            return data;
        }

        public static IQueryable<Models.Base.DistanceEntity<Data.Business>> Distance(SizeUpContext context, LatLng latLng)
        {           
            var data = Get(context)
                       .Where(i=> i.MatchLevel == "0")
                       .Select(i => new Models.Base.DistanceEntity<Data.Business>
                       {
                           Distance = System.Math.Pow(System.Math.Pow(((double)i.Lat.Value - latLng.Lat) * 69.1, 2) + System.Math.Pow(((double)i.Long.Value - latLng.Lng) * (double)(System.Data.Objects.SqlClient.SqlFunctions.Cos(latLng.Lat / 57.3) * 69.1), 2), 0.5),
                           Entity = i
                       });
            return data;
        }

        public static IQueryable<Data.Business> In(SizeUpContext context, Core.Geo.BoundingBox boundingBox)
        {
            var data = Get(context)
                       .Where(i => i.MatchLevel == "0")
                       .Where(i => i.Lat < (decimal)boundingBox.NorthEast.Lat && i.Lat > (decimal)boundingBox.SouthWest.Lat && i.Long > (decimal)boundingBox.SouthWest.Lng && i.Long < (decimal)boundingBox.NorthEast.Lng);
            return data;
        }


    }
}
