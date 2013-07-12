using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public abstract class Filter<T>
    {
        public abstract Expression<Func<T, bool>> Expression { get; }
    }
}
