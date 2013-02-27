using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.Objects.DataClasses;

namespace SizeUp.Core.DataLayer
{
    public class IndustryData : Base
    {
        /*
        public static IQueryable<PlaceValues<IQueryable<IndustryDataByCity>, IQueryable<IndustryDataByCounty>, IQueryable<IndustryDataByMetro>, IQueryable<IndustryDataByState>, IQueryable<IndustryDataByNation>>> Get(SizeUpContext context, long industryId, long placeId)
        {
            var data = context.CityCountyMappings
               .Where(i=>i.Id == placeId)
               .Select(i => new PlaceValues<IQueryable<IndustryDataByCity>, IQueryable<IndustryDataByCounty>, IQueryable<IndustryDataByMetro>, IQueryable<IndustryDataByState>, IQueryable<IndustryDataByNation>>
               {
                   City = i.City.IndustryDataByCities
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByCity>(),

                   County = i.County.IndustryDataByCounties
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByCounty>(),

                   Metro = i.County.Metro.IndustryDataByMetroes
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByMetro>(),

                   State = i.County.State.IndustryDataByStates
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByState>(),

                   Nation = context.IndustryDataByNations
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByNation>()
               }).AsQueryable();
            return data;
        }
        */
         
        
        public static IQueryable<Models.IndustryData> Get(SizeUpContext context, long industryId)
        {
            var data = context.CityCountyMappings
               .Select(i => new Models.IndustryData
               {
                   Place = i,

                   City = i.City.IndustryDataByCities
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByCity>(),

                   County = i.County.IndustryDataByCounties
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByCounty>(),

                   Metro = i.County.Metro.IndustryDataByMetroes
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByMetro>(),

                   State = i.County.State.IndustryDataByStates
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByState>(),

                   Nation = context.IndustryDataByNations
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount).AsQueryable<IndustryDataByNation>()
               }).AsQueryable<Models.IndustryData>();
            return data;
        }
    }
}
