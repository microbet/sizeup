using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class Industry 
    {
        public class Active : Filter<Data.Industry>
        {
            public override Expression<Func<Data.Industry, bool>> Expression
            {
                get
                {
                    return i => i.IsActive && !i.IsDisabled;
                }
            }
        }
    }
}
