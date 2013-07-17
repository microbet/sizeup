using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer
{
    public class IndustryData
    {
        public static IQueryable<Data.IndustryData> Get(SizeUpContext context)
        {
            return Core.DataLayer.Industry.Get(context)
                .SelectMany(i => i.IndustryDatas)
                .Where(i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter);
        }

        public static IQueryable<Data.IndustryData> GetMinimumBusinessCount(SizeUpContext context)
        {
            return Get(context)
                .Where(i => i.GeographicLocation.BusinessDatas.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive && b.Business.InBusiness).Count() > CommonFilters.MinimumBusinessCount);
        }
    }
}
