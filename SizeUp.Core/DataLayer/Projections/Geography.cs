using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.Objects.SqlClient;


namespace SizeUp.Core.DataLayer.Projections
{
    public static class Geography
    {
        public class Centroid : Projection<Data.Geography, KeyValue<Data.GeographicLocation, Core.Geo.LatLng>>
        {
            public override Expression<Func<Data.Geography, KeyValue<Data.GeographicLocation, Core.Geo.LatLng>>> Expression
            {
                get
                {
                    return i => new KeyValue<Data.GeographicLocation, Core.Geo.LatLng>
                    {
                        Key = i.GeographicLocation,
                        Value = new Geo.LatLng
                        {
                            Lat = i.CenterLat.Value,
                            Lng = i.CenterLong.Value
                        }
                    };
                }
            }
        }

        public class BoundingBox : Projection<Data.Geography, KeyValue<Data.GeographicLocation, Core.Geo.BoundingBox>>
        {
            public override Expression<Func<Data.Geography, KeyValue<Data.GeographicLocation, Core.Geo.BoundingBox>>> Expression
            {
                get
                {
                    return i => new KeyValue<Data.GeographicLocation, Core.Geo.BoundingBox>
                    {
                        Key = i.GeographicLocation,
                        Value = new Geo.BoundingBox
                        {
                            SouthWest = new Geo.LatLng
                            {
                                Lat = i.South,
                                Lng = i.West
                            },
                            NorthEast = new Geo.LatLng
                            {
                                Lat = i.North,
                                Lng = i.East
                            }
                        }
                    };
                }
            }
        }
    }
}
