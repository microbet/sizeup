using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class CostEffectiveness
    {
        public static BarChartItem<double?> Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
               .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
               .Where(i => i.CostEffectiveness != null)
               .Where(i => i.GeographicLocationId == geographicLocationId);
            return data
                .Select(new Projections.CostEffectiveness.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long geographicLocationId, double value)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                        .Where(i => i.IndustryId == industryId)
                        .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                        .Where(i => i.GeographicLocationId == geographicLocationId);

            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)((((value - i.CostEffectiveness) / (double)i.CostEffectiveness)) * 100)
            })
            .FirstOrDefault(); 
        }

        public static List<Band<double>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            var output = data
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0)
                .Select(i => i.Bands.Where(b => b.Attribute.Name == IndustryAttribute.CostEffectiveness).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault())
                .ToList()
                .NTileDescending(i => i.Min, bands)
                .Select(i => new Band<double>() { Min = i.Min(v => v.Min), Max = i.Max(v => v.Max) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
