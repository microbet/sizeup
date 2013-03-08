using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Place
    {
        public long? Id { get; set; }
        public string DisplayName { get; set; }
        public City City { get; set; }
        public County County { get; set; }
        public Metro Metro { get; set; }
        public State State { get; set; }
    }
}
