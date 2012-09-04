using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core;
using SizeUp.Data;


namespace SizeUp.Core.DataAccess
{
    public static class DemographicsData
    {
        public static IQueryable<DemographicsByState> GetStates(SizeUpContext context)
        {
            return context.DemographicsByStates
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<DemographicsByState> GetStates(SizeUpContext context, long stateId)
        {
            return context.DemographicsByStates
                .Where(i => i.StateId == stateId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<DemographicsByCounty> GetCounties(SizeUpContext context)
        {
            return context.DemographicsByCounties
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<DemographicsByCounty> GetCounty(SizeUpContext context, long countyId)
        {
            return context.DemographicsByCounties
                .Where(i => i.CountyId == countyId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }


        public static IQueryable<DemographicsByCity> GetCities(SizeUpContext context)
        {
            return context.DemographicsByCities
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<DemographicsByCity> GetCity(SizeUpContext context, long cityId)
        {
            return context.DemographicsByCities
                .Where(i => i.CityId == cityId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }


        public static IQueryable<DemographicsByZip> GetZipCodes(SizeUpContext context)
        {
            return context.DemographicsByZips
                .Where(i => i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }

        public static IQueryable<DemographicsByZip> GetZipCode(SizeUpContext context, long zipCodeId)
        {
            return context.DemographicsByZips
                .Where(i => i.ZipCodeId == zipCodeId && i.Year == TimeSlice.Year && i.Quarter == TimeSlice.Quarter);
        }
    }
}
