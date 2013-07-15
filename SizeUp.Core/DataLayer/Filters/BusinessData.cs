using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class BusinessData
    {
        public class Current : Filter<Data.BusinessData>
        {
            public override Expression<Func<Data.BusinessData, bool>> Expression
            {
                get
                {
                    return i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter;
                }
            }
        }
    }
}
