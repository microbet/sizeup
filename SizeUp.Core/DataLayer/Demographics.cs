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

        public static Models.Demographics Get(SizeUpContext context, long geographicLocationId)
        {
            Models.Demographics output = Get(context)
                .Where(i => i.GeographicLocationId == geographicLocationId)
                .Select(new Projections.Demographics.Default().Expression)
                .FirstOrDefault();
            return output;
        }
    }
}
