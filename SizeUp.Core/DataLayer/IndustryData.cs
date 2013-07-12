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
                .Where(new Filters.IndustryData.Current().Expression);
        }
    }
}
