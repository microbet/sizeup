using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer
{
    public class JobChange
    {
        public static JobChangeChartItem Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            //var data = Core.DataLayer.IndustryData.Get(context)
            //    .Where(i => i.IndustryId == industryId)
            //    .Where(i => i.GeographicLocationId == geographicLocationId);
            var data = context.IndustryDatas
             .Where(i => i.Year == 2015 && i.Quarter == 2 && i.Industry.IsActive && !i.Industry.IsDisabled)
             .Where(i => i.IndustryId == industryId)
             .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
            .Select(new Projections.JobChange.Chart().Expression)
            .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.County);

            //var raw = Core.DataLayer.IndustryData.Get(context)
            //    .Where(i => i.IndustryId == industryId)
            //    .Where(i => i.GeographicLocation.Granularity.Name == gran)
            //    .Where(i => i.NetJobChange != null && i.NetJobChange > 0);

            var raw = context.IndustryDatas
             .Where(i => i.Year == 2015 && i.Quarter == 2 && i.Industry.IsActive && !i.Industry.IsDisabled)
           .Where(i => i.IndustryId == industryId)
           .Where(i => i.GeographicLocation.Granularity.Name == gran)
           .Where(i => i.NetJobChange != null && i.NetJobChange > 0);

            var value = raw.Where(i => i.GeographicLocationId == geographicLocationId).Select(i => i.NetJobChange);

            raw = raw.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));
            output = raw.Select(i => new
            {
                i.GeographicLocation.LongName,
                Total = raw.Count(),
                Filtered = raw.Count(c => c.NetJobChange >= value.FirstOrDefault())
            })
            .Select(i => new PercentileItem
            {
                Name = i.LongName,
                Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
            }).FirstOrDefault();

            return output;
        }
    }
}
