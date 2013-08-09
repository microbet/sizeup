using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;


namespace SizeUp.Core.DataLayer
{
    public class Turnover
    {
        public static TurnoverChartItem Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
               .Where(i => i.IndustryId == industryId)
               .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
                .Select(new Projections.Turnover.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.County);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.TurnoverRate != null && i.TurnoverRate > 0);

            var value = raw.Where(i => i.GeographicLocationId == geographicLocationId).Select(i => i.TurnoverRate);

            raw = raw.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));
            output = raw.Select(i => new
            {
                i.GeographicLocation.LongName,
                Total = raw.Count(),
                Filtered = raw.Count(c => c.TurnoverRate >= value.FirstOrDefault())
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
