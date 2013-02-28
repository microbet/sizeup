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
        public static IQueryable<Models.Base.BusinessData> Get(SizeUpContext context, long industryId)
        {
            var data = context.CityCountyMappings
               .Select(i => new Models.Base.BusinessData
               {
                   Place = i,

                   City = i.City.BusinessDataByCities
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter).AsQueryable<BusinessDataByCity>(),

                   County = i.County.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter).AsQueryable<BusinessDataByCounty>(),

                   Metro = i.County.Metro.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter).AsQueryable<BusinessDataByCounty>(),

                   State = i.County.State.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter).AsQueryable<BusinessDataByCounty>(),

                   Nation = context.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter).AsQueryable<BusinessDataByCounty>(),
               }).AsQueryable<Models.Base.BusinessData>();
            return data;
        }
    }
}
