
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Division : GeographicLocation
    {
        public long? Id { get; set; }
        public string RegionName { get; set; }
        public string Name { get; set; }
    }
}
