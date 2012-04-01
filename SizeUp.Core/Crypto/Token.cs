using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.Crypto
{
    public class Token<T>
    {
        public T Value { get; set; }
        public DateTime Salt { get; set; }
    }
}
