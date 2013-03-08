﻿using System;
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
        public SqlGeography SqlGeography { get { return SqlGeography.Parse(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", SouthWest.Lng, NorthEast.Lng, SouthWest.Lat, NorthEast.Lat)); } }
        public DbGeography DbGeography { get { return DbGeography.FromText(string.Format("POLYGON (({0} {2}, {1} {2}, {1} {3}, {0} {3}, {0} {2}))", SouthWest.Lng, NorthEast.Lng, SouthWest.Lat, NorthEast.Lat)); } }
        public BoundingBox(PointF southWest, PointF northEast)
        {
            SouthWest = new LatLng { Lat = southWest.Y, Lng = southWest.X };
            NorthEast = new LatLng { Lat = northEast.Y, Lng = northEast.X }; ;
        }
    }
}
