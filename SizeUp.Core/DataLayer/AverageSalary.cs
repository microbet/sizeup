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
    public class AverageSalary
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
                .Where(i => i.AverageAnnualSalary != null)
                .Where(i => i.GeographicLocationId == geographicLocationId);
            
            return data
                .Select(new Projections.AverageSalary.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long geographicLocationId, long value)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                        .Where(i => i.IndustryId == industryId)
                        .Where(i => i.GeographicLocationId == geographicLocationId);


            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)((((value - i.AverageAnnualSalary) / (decimal)i.AverageAnnualSalary)) * 100)
            })
                .FirstOrDefault(); 
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            var output = data.Select(i => i.AverageAnnualSalary)
                .Where(i => i != null && i > 0)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
