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
    public class BusinessData
    {
        public static IQueryable<Data.BusinessData> Get(SizeUpContext context)
        {
            return Core.DataLayer.Industry.Get(context)
                .SelectMany(i => i.BusinessDatas)
                .Where(new Filters.BusinessData.Current().Expression);
        }
    }
}
