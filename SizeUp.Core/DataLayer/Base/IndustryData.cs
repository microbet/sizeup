using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Objects.DataClasses;

namespace SizeUp.Core.DataLayer.Base
{
    public class IndustryData : Base
    {
        public static IQueryable<Models.Base.IndustryData> GetMinimumBusinessCount(SizeUpContext context, long industryId)
        {
            var data = context.CityCountyMappings
               .Select(i => new Models.Base.IndustryData
               {
                   Place = i,

                   City = i.City.IndustryDataByCities
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                       .AsQueryable<IndustryDataByCity>(),

                   County = i.County.IndustryDataByCounties
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                       .AsQueryable<IndustryDataByCounty>(),

                   Metro = i.County.Metro.IndustryDataByMetroes
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                       .AsQueryable<IndustryDataByMetro>(),

                   State = i.County.State.IndustryDataByStates
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                       .AsQueryable<IndustryDataByState>(),

                   Nation = context.IndustryDataByNations
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                       .AsQueryable<IndustryDataByNation>()
               }).AsQueryable<Models.Base.IndustryData>();
            return data;
        }

        public static IQueryable<Models.Base.IndustryData> Get(SizeUpContext context, long industryId)
        {
            var data = context.CityCountyMappings
               .Select(i => new Models.Base.IndustryData
               {
                   Place = i,

                   City = i.City.IndustryDataByCities
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .AsQueryable<IndustryDataByCity>(),

                   County = i.County.IndustryDataByCounties
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .AsQueryable<IndustryDataByCounty>(),

                   Metro = i.County.Metro.IndustryDataByMetroes
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .AsQueryable<IndustryDataByMetro>(),

                   State = i.County.State.IndustryDataByStates
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .AsQueryable<IndustryDataByState>(),

                   Nation = context.IndustryDataByNations
                       .Where(d => d.IndustryId == industryId)
                       .Where(d => d.Year == Year && d.Quarter == Quarter)
                       .AsQueryable<IndustryDataByNation>()
               }).AsQueryable<Models.Base.IndustryData>();
            return data;
        }





        public static IQueryable<IndustryDataByZip> ZipCode(SizeUpContext context)
        {
            var data = context.IndustryDataByZips
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByCity> City(SizeUpContext context)
        {
            var data = context.IndustryDataByCities
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByCounty> County(SizeUpContext context)
        {
            var data = context.IndustryDataByCounties
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByMetro> Metro(SizeUpContext context)
        {
            var data = context.IndustryDataByMetroes
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByState> State(SizeUpContext context)
        {
            var data = context.IndustryDataByStates
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByNation> Nation(SizeUpContext context)
        {
            var data = context.IndustryDataByNations
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }



    }
}
