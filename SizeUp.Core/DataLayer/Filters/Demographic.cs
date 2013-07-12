using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class Demographic
    {
        public class Current : Filter<Data.Demographic>
        {
            public override Expression<Func<Data.Demographic, bool>> Expression
            {
                get
                {
                    return i => i.Year == CommonFilters.TimeSlice.Demographics.Year && i.Quarter == CommonFilters.TimeSlice.Demographics.Quarter;
                }
            }
        }
    }
}
