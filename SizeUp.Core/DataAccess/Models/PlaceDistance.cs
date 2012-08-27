using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataAccess.Models
{
    public class PlaceDistance
    {
        public double CityDistance { get; set; }
        public double CountyDistance { get; set; }
        public Locations Entity { get; set; }
    }
}
