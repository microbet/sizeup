using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer.Base
{
    public class Business : Base
    {
        public static IQueryable<Data.Business> Get(SizeUpContext context)
        {
            var data = context.Businesses
                       .Where(d => d.IsActive && d.MatchLevel == "0");
            return data;
        }

        public static IQueryable<Models.Base.DistanceEntity<Data.Business>> Distance(SizeUpContext context, Models.LatLng latLng)
        {
            var scalar = 69.1 * System.Math.Cos(latLng.Lat / 57.3);
            var data = Get(context)
                       .Select(i => new Models.Base.DistanceEntity<Data.Business>
                       {
                           Distance = System.Math.Pow(System.Math.Pow(((double)i.Lat.Value - latLng.Lat) * 69.1, 2) + System.Math.Pow(((double)i.Long.Value - latLng.Lng) * scalar, 2), 0.5),
                           Entity = i
                       });
            return data;
        }


    }
}
