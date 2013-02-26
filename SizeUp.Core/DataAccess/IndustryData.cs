using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core;
using SizeUp.Data;

namespace SizeUp.Core.DataAccess
{
    public static class IndustryData
    {
        public static IQueryable<IndustryDataByNation> GetNational(SizeUpContext context)
        {
            return context.IndustryDataByNations
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByNation> GetNational(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByNations
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByState> GetStates(SizeUpContext context)
        {
            return context.IndustryDataByStates
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByState> GetStates(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByStates
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByState> GetState(SizeUpContext context, long industryId, long stateId)
        {
            return context.IndustryDataByStates
                .Where(i => i.StateId == stateId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }


        public static IQueryable<IndustryDataByMetro> GetMetros(SizeUpContext context)
        {
            return context.IndustryDataByMetroes
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByMetro> GetMetros(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByMetroes
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByMetro> GetMetro(SizeUpContext context, long industryId, long metroId)
        {
            return context.IndustryDataByMetroes
                .Where(i =>i.MetroId == metroId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }



        public static IQueryable<IndustryDataByCounty> GetCounties(SizeUpContext context)
        {
            return context.IndustryDataByCounties
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByCounty> GetCounties(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByCounties
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByCounty> GetCounty(SizeUpContext context, long industryId, long countyId)
        {
            return context.IndustryDataByCounties
                .Where(i =>i.CountyId == countyId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }


        public static IQueryable<IndustryDataByCity> GetCities(SizeUpContext context)
        {
            return context.IndustryDataByCities
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByCity> GetCities(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByCities
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByCity> GetCity(SizeUpContext context, long industryId, long cityId)
        {
            return context.IndustryDataByCities
                .Where(i => i.CityId == cityId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                .Where(i => i.Industry.BusinessDataByCities.Where(b => b.Business.IsActive && b.CityId == cityId).Count() >= 3);
        }

        public static IQueryable<IndustryDataByCity> GetCity(SizeUpContext context, long cityId)
        {
            return context.IndustryDataByCities
                .Where(i => i.CityId == cityId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter)
                .Where(i => i.Industry.BusinessDataByCities.Where(b => b.Business.IsActive && b.CityId == cityId).Count() >= 3);
        }


        public static IQueryable<IndustryDataByZip> GetZipCodes(SizeUpContext context)
        {
            return context.IndustryDataByZips
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByZip> GetZipCodes(SizeUpContext context, long industryId)
        {
            return context.IndustryDataByZips
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<IndustryDataByZip> GetZipCode(SizeUpContext context, long industryId, long zipCodeId)
        {
            return context.IndustryDataByZips
                .Where(i =>i.ZipCodeId == zipCodeId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }
    }
}
