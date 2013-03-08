using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.Geo;

namespace SizeUp.Core.DataLayer.Models
{
    public class BoundingBox
    {
        public LatLng NorthEast { get; set; }
        public LatLng SouthWest { get; set; }
    }
}
