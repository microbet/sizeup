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
        public static IQueryable<IndustryDataByZip> ZipCode(SizeUpContext context)
        {
            var data = context.IndustryDataByZips
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }

        public static IQueryable<IndustryDataByCity> City(SizeUpContext context)
        {
            var data = context.IndustryDataByCities
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }

        public static IQueryable<IndustryDataByCounty> County(SizeUpContext context)
        {
            var data = context.IndustryDataByCounties
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }

        public static IQueryable<IndustryDataByMetro> Metro(SizeUpContext context)
        {
            var data = context.IndustryDataByMetroes
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }

        public static IQueryable<IndustryDataByState> State(SizeUpContext context)
        {
            var data = context.IndustryDataByStates
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }

        public static IQueryable<IndustryDataByNation> Nation(SizeUpContext context)
        {
            var data = context.IndustryDataByNations
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive);
            return data;
        }


        public static IQueryable<IndustryDataByZip> ZipCodeMinBusinessCount(SizeUpContext context)
        {
            var data = context.IndustryDataByZips
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive)
                       .Where(i => i.ZipCode.BusinessDataByZips.Where(b => b.Year == TimeSlice.Industry.Year && b.Quarter == TimeSlice.Industry.Quarter && b.IndustryId == i.IndustryId && b.Business.IsActive).Count() > MinimumBusinessCount);
            return data;
        }

        public static IQueryable<IndustryDataByCity> CityMinBusinessCount(SizeUpContext context)
        {
            var data = context.IndustryDataByCities
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive)
                       .Where(i => i.City.BusinessDataByCities.Where(b => b.Year == TimeSlice.Industry.Year && b.Quarter == TimeSlice.Industry.Quarter && b.IndustryId == i.IndustryId && b.Business.IsActive).Count() > MinimumBusinessCount);
            return data;
        }

        public static IQueryable<IndustryDataByCounty> CountyMinBusinessCount(SizeUpContext context)
        {
            var data = context.IndustryDataByCounties
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive)
                       .Where(i => i.County.BusinessDataByCounties.Where(b => b.Year == TimeSlice.Industry.Year && b.Quarter == TimeSlice.Industry.Quarter && b.IndustryId == i.IndustryId && b.Business.IsActive).Count() > MinimumBusinessCount);
            return data;
        }

        public static IQueryable<IndustryDataByMetro> MetroMinBusinessCount(SizeUpContext context)
        {
            var data = context.IndustryDataByMetroes
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive)
                       .Where(i => i.Metro.BusinessDataByCounties.Where(b => b.Year == TimeSlice.Industry.Year && b.Quarter == TimeSlice.Industry.Quarter && b.IndustryId == i.IndustryId && b.Business.IsActive).Count() > MinimumBusinessCount);
            return data;
        }

        public static IQueryable<IndustryDataByState> StateMinBusinessCount(SizeUpContext context)
        {
            var data = context.IndustryDataByStates
                       .Where(d => d.Year == TimeSlice.Industry.Year && d.Quarter == TimeSlice.Industry.Quarter && d.Industry.IsActive)
                       .Where(i => i.State.BusinessDataByCounties.Where(b => b.Year == TimeSlice.Industry.Year && b.Quarter == TimeSlice.Industry.Quarter && b.IndustryId == i.IndustryId && b.Business.IsActive).Count() > MinimumBusinessCount);
            return data;
        }
    }
}
