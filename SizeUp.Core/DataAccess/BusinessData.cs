using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core;
using SizeUp.Data;

namespace SizeUp.Core.DataAccess
{
    public static class BusinessData
    {
        public static IQueryable<BusinessDataByCounty> GetByNation(SizeUpContext context, long industryId)
        {
            return context.BusinessDataByCounties
                .Where(i => i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<BusinessDataByCounty> GetByState(SizeUpContext context, long industryId, long stateId)
        {
            return context.BusinessDataByCounties
                .Where(i => i.StateId == stateId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<BusinessDataByCounty> GetByMetro(SizeUpContext context, long industryId, long metroId)
        {
            return context.BusinessDataByCounties
                .Where(i => i.MetroId == metroId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<BusinessDataByCounty> GetByCounty(SizeUpContext context, long industryId, long countyId)
        {
            return context.BusinessDataByCounties
                .Where(i => i.CountyId == countyId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<BusinessDataByCity> GetByCity(SizeUpContext context, long industryId, long cityId)
        {
            return context.BusinessDataByCities
                .Where(i => i.CityId == cityId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<BusinessDataByZip> GetByZipCode(SizeUpContext context, long industryId, long zipCodeId)
        {
            return context.BusinessDataByZips
                .Where(i => i.ZipCodeId == zipCodeId && i.IndustryId == industryId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }
    }
}
