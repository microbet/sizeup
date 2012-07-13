using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Data.Models
{
    public class Locations
    {
        public City City { get; set; }
        public County County { get; set; }
        public Metro Metro { get; set; }
        public State State { get; set; }
    }
}
