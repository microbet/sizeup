using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class IndustryData 
    {
        public class Current : Filter<Data.IndustryData>
        {
            public override Expression<Func<Data.IndustryData, bool>> Expression
            {
                get
                {
                    return i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter;
                }
            }
        }

        public class MinimumBusinessCount : Filter<Data.IndustryData>
        {
            public override Expression<Func<Data.IndustryData, bool>> Expression
            {
                get
                {
                    return i => i.GeographicLocation.BusinessDatas
                        .AsQueryable()
                        .Where(new BusinessData.Current().Expression)
                        .Where(g=>g.IndustryId == i.IndustryId)
                        .Count(b=>b.Business.IsActive) > CommonFilters.MinimumBusinessCount;
                }
            }
        }
    }
}
