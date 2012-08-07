using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataAccess.Models
{
    public class EntityDistance<T>
    {
        public double Distance { get; set; }
        public T Entity { get; set; }
    }
}
