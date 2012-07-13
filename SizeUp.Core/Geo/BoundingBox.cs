using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SizeUp.Core.Geo
{
    public class BoundingBox
    {
        public PointF NorthEast { get; set; }
        public PointF SouthWest { get; set; }
        public BoundingBox(PointF southWest, PointF northEast)
        {
            SouthWest = southWest;
            NorthEast = northEast;
        }
    }
}
