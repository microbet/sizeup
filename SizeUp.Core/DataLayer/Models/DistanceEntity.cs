using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class DistanceEntity<E>
    {
        public class DistanceEntityFilter
        {
            protected Core.Geo.LatLng Center = null;

            public DistanceEntityFilter(Geo.LatLng Center)
            {
                this.Center = Center;
            }

            public Expression<Func<Models.KeyValue<E, Geo.LatLng>, DistanceEntity<E>>> Projection
            {
                get
                {
                    return i => new DistanceEntity<E>
                       {
                           Distance = System.Math.Pow(System.Math.Pow(((double)i.Value.Lat - Center.Lat) * 69.1, 2) + System.Math.Pow(((double)i.Value.Lng - Center.Lng) * (double)(System.Data.Objects.SqlClient.SqlFunctions.Cos(Center.Lat / 57.3) * 69.1), 2), 0.5),
                           Entity = i.Key
                       };
                }
            }
        }
        public double Distance { get; set; }
        public E Entity { get; set; }
    }
}
