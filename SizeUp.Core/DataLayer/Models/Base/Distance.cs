using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models.Base
{
    public class DistanceEntity<E>
    {
        public double Distance { get; set; }
        public E Entity { get; set; }
    }
}
