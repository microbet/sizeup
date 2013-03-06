using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer.Base
{
    public class BusinessData : Base
    {
        public static IQueryable<BusinessDataByCity> City(SizeUpContext context)
        {
            var data = context.BusinessDataByCities
                       .Where(d => d.Business.IsActive && d.Business.IndustryId == d.IndustryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<BusinessDataByCounty> County(SizeUpContext context)
        {
            var data = context.BusinessDataByCounties
                       .Where(d => d.Business.IsActive && d.Business.IndustryId == d.IndustryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }
    }
}
