using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class Band<T>
    {
        public T Min {get;set;}
        public T Max { get; set; }
    }
}
