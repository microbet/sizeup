using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer
{
    public class BusinessData : Base
    {
        public IQueryable Get(SizeUpContext context, long industryId, long placeId)
        {
            var data = context.CityCountyMappings
               .Where(i => i.Id == placeId)
               .Select(i => new
               {
                   City = i.City.BusinessDataByCities
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.YearAppeared != null && d.YearEstablished != null)
                       .Where(d => d.Year == Year && d.Quarter == Quarter),

                   County = i.County.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.YearAppeared != null && d.YearEstablished != null)
                       .Where(d => d.Year == Year && d.Quarter == Quarter),

                   Metro = i.County.Metro.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.YearAppeared != null && d.YearEstablished != null)
                       .Where(d => d.Year == Year && d.Quarter == Quarter),

                   State = i.County.State.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.YearAppeared != null && d.YearEstablished != null)
                       .Where(d => d.Year == Year && d.Quarter == Quarter),

                   Nation = context.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.YearAppeared != null && d.YearEstablished != null)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
               });
            return data;
        }
    }
}
