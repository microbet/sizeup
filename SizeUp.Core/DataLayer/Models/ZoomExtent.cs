using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class ZoomExtent
    {
        public long? PlaceId { get; set; }
        public int? County { get; set; }
        public int? Metro { get; set; }
        public int? State { get; set; }
    }
}
