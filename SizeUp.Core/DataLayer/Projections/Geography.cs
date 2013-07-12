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
        public class Centroid : Projection<Data.Geography, Core.Geo.LatLng>
        {
            public override Expression<Func<Data.Geography, Core.Geo.LatLng>> Expression
            {
                get
                {
                    return i => new Geo.LatLng
                    {
                        Lat = i.CenterLat.Value,
                        Lng = i.CenterLong.Value
                    };
                }
            }
        }

        public class BoundingBox : Projection<Data.Geography, Core.Geo.BoundingBox>
        {
            public override Expression<Func<Data.Geography, Core.Geo.BoundingBox>> Expression
            {
                get
                {
                    return i => new Geo.BoundingBox
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
                    };
                }
            }
        }


        public class ZoomExtent : Projection<Data.Place, Models.ZoomExtent>
        {
            public long MapWidth {get;set;}
            public ZoomExtent(long mapWidth)
            {
                this.MapWidth = mapWidth;
            }

            public override Expression<Func<Data.Place, Models.ZoomExtent>> Expression
            {
                get
                {
                    double GLOBE_WIDTH = 256; // a constant in Google's map projection
                    double ln2 = Math.Log(2);

                    return i => new Models.ZoomExtent
                    {
                        PlaceId = i.Id,
                        County = i.County.GeographicLocation.Geographies.Where(g=>g.GeographyClass.Name == Geo.GeographyClass.Calculation)
                            .Select(g=>(int)Math.Round(SqlFunctions.Log(MapWidth * 360 / (g.East - g.West) / GLOBE_WIDTH).Value / ln2) - 1).FirstOrDefault(),

                        Metro = i.County.Metro.GeographicLocation.Geographies.Where(g => g.GeographyClass.Name == Geo.GeographyClass.Calculation)
                            .Select(g => (int)Math.Round(SqlFunctions.Log(MapWidth * 360 / (g.East - g.West) / GLOBE_WIDTH).Value / ln2) - 1).FirstOrDefault(),

                        State = i.County.State.GeographicLocation.Geographies.Where(g => g.GeographyClass.Name == Geo.GeographyClass.Calculation)
                            .Select(g => (int)Math.Round(SqlFunctions.Log(MapWidth * 360 / (g.East - g.West) / GLOBE_WIDTH).Value / ln2) - 1).FirstOrDefault()
                    };
                }
            }
        }
    }
}
