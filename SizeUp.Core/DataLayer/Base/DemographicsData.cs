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
    public class DemographicsData : Base
    {
        public static IQueryable<DemographicsByZip> ZipCode(SizeUpContext context)
        {
            var data = context.DemographicsByZips
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<DemographicsByCity> City(SizeUpContext context)
        {
            var data = context.DemographicsByCities
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<DemographicsByCounty> County(SizeUpContext context)
        {
            var data = context.DemographicsByCounties
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<DemographicsByMetro> Metro(SizeUpContext context)
        {
            var data = context.DemographicsByMetroes
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<DemographicsByState> State(SizeUpContext context)
        {
            var data = context.DemographicsByStates
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }
        
    }
}
