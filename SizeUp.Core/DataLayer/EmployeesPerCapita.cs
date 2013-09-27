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
    public class EmployeesPerCapita 
    {
        public static BarChartItem<double?> Chart(SizeUpContext context, long industryId, long geographicLocationId )
        {
            var data = Core.DataLayer.IndustryData.Get(context)
               .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
               .Where(i => i.EmployeesPerCapita != null)
               .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
                .Select(new Projections.EmployeesPerCapita.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.City);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);

            var value = raw.Where(i => i.GeographicLocationId == geographicLocationId).Select(i => i.EmployeesPerCapita);

            raw = raw.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));
            output = raw.Select(i => new
            {
                i.GeographicLocation.LongName,
                Total = raw.Count(),
                Filtered = raw.Count(c => c.EmployeesPerCapita >= value.FirstOrDefault())
            })
            .Select(i => new PercentileItem
            {
                Name = i.LongName,
                Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
            }).FirstOrDefault();

            return output;
        }

        public static List<Band<double>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            var output = data
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0)
                .Select(i => i.Bands.Where(b => b.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault())
                .ToList()
                .NTileDescending(i => i.Min, bands)
                .Select(i => new Band<double>() { Min = i.Min(v => v.Min), Max = i.Max(v => v.Max) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
