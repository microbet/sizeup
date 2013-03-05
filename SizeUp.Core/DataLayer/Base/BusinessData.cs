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

        public static IQueryable<Models.Base.BusinessData> GetMinimumBusinessCount(SizeUpContext context, long industryId)
        {
            var data = context.CityCountyMappings
               .Select(i => new Models.Base.BusinessData
               {
                   Place = i,

                   City = i.City.BusinessDataByCities
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive && b.Year == Year && b.Quarter == Quarter).Count() >= MinimumBusinessCount)
                       .AsQueryable<BusinessDataByCity>(),

                   County = i.County.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive && b.Year == Year && b.Quarter == Quarter).Count() >= MinimumBusinessCount)
                       .AsQueryable<BusinessDataByCounty>(),

                   Metro = i.County.Metro.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive && b.Year == Year && b.Quarter == Quarter).Count() >= MinimumBusinessCount)
                       .AsQueryable<BusinessDataByCounty>(),

                   State = i.County.State.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive && b.Year == Year && b.Quarter == Quarter).Count() >= MinimumBusinessCount)
                       .AsQueryable<BusinessDataByCounty>(),

                   Nation = context.BusinessDataByCounties
                       .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive && b.Year == Year && b.Quarter == Quarter).Count() >= MinimumBusinessCount)
                       .AsQueryable<BusinessDataByCounty>()
               }).AsQueryable<Models.Base.BusinessData>();
            return data;
        }

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
