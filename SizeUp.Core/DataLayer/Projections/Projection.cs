using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Projections
{
    public abstract class Projection<I,O>
    {
        public abstract Expression<Func<I, O>> Expression { get; }
    }
}
