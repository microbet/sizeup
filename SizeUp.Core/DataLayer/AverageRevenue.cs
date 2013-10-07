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
    public class AverageRevenue
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
                .Where(i => i.AverageRevenue != null)
                .Where(i => i.GeographicLocationId == geographicLocationId);
            
            return data
                .Select(new Projections.AverageRevenue.Chart().Expression)
                .FirstOrDefault();
        }


        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, long value)
        {

            var data = Core.DataLayer.BusinessData.Get(context)
                        .Where(i => i.IndustryId == industryId)
                        .Where(i => i.Revenue != null && i.Revenue > 0)
                        .Where(i => i.GeographicLocationId == geographicLocationId);
            
            var output = data.Select(i => new
            {
                Name = i.GeographicLocation.LongName,
                Filtered = data.Count(c => c.Revenue <= value),
                Total = data.Count()
            }).FirstOrDefault();


            return output!= null && output.Total > CommonFilters.MinimumBusinessCount ? new PercentileItem
            {
                Name = output.Name,
                Percentile = (((decimal)output.Filtered / ((decimal)output.Total + 1) * 100))
            } : null;
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            var output = data
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0)
                .Select(i => i.Bands.Where(b => b.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault())
                .ToList()
                .NTileDescending(i => i.Min, bands)
                .Select(i => new Band<long>() { Min = (long)i.Min(v => v.Min), Max = (long)i.Max(v => v.Max) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
