using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class Demographics
    {
        public static IQueryable<Data.Demographic> Get(SizeUpContext context)
        {
            return context.Demographics.Where(i => i.Year == CommonFilters.TimeSlice.Demographics.Year && i.Quarter == CommonFilters.TimeSlice.Demographics.Quarter);
        }

        public static IQueryable<Data.Demographic> Get(SizeUpContext context, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);
            return Get(context)
                .Where(i => i.GeographicLocation.Granularity.Name == gran);
        }

        public static Models.Demographics Get(SizeUpContext context, long id, Granularity granularity)
        {
            Models.Demographics output = Get(context, granularity)
                .Where(i => i.GeographicLocationId == id)
                .Select(new Projections.Demographics.Default().Expression)
                .FirstOrDefault();
            return output;
        }
    }
}
