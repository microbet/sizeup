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
        public PointF NorthEast { get; set; }
        public PointF SouthWest { get; set; }
        public SqlGeography SqlGeography { get { return SqlGeography.Parse(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", SouthWest.X, NorthEast.X, SouthWest.Y, NorthEast.Y)); } }
        public DbGeography DbGeography { get { return DbGeography.FromText(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", SouthWest.X, NorthEast.X, SouthWest.Y, NorthEast.Y)); } }
        public BoundingBox(PointF southWest, PointF northEast)
        {
            SouthWest = southWest;
            NorthEast = northEast;
        }
    }
}
