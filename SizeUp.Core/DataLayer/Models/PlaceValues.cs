using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class PlaceValues<T>
    {
        public T City { get; set; }
        public T County { get; set; }
        public T Metro { get; set; }
        public T State { get; set; }
        public T Nation { get; set; }
    }

    public class PlaceValues<C,CO,M,S,N>
    {
        public C City { get; set; }
        public CO County { get; set; }
        public M Metro { get; set; }
        public S State { get; set; }
        public N Nation { get; set; }
    }
}
