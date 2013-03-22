using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.SqlServer.Types;
using System.Data.Spatial;

namespace SizeUp.Core.Geo
{
    public class BoundingBox
    {
        public LatLng NorthEast { get; set; }
        public LatLng SouthWest { get; set; }
        public BoundingBox()
        {

        }
        public BoundingBox(PointF southWest, PointF northEast)
        {
            SouthWest = new LatLng { Lat = southWest.Y, Lng = southWest.X };
            NorthEast = new LatLng { Lat = northEast.Y, Lng = northEast.X }; ;
        }

        private double BoundLat(double lat)
        {
            return Math.Min(Math.Max(0, lat), 90);
        }

        private double BoundLng(double lng)
        {
            return Math.Min(Math.Max(-180, lng), 0);
        }

        public SqlGeography GetSqlGeography()
        {
            return SqlGeography.Parse(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", BoundLng(SouthWest.Lng), BoundLng(NorthEast.Lng), BoundLat(SouthWest.Lat), BoundLat(NorthEast.Lat)));
        }

        public DbGeography GetDbGeography()
        {
            return DbGeography.FromText(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", BoundLng(SouthWest.Lng), BoundLng(NorthEast.Lng), BoundLat(SouthWest.Lat), BoundLat(NorthEast.Lat)));
        }
    }
}
