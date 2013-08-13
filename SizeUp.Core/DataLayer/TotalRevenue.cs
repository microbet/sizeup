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
    public class TotalRevenue
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long geographicLocationId )
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
                .Where(i => i.TotalRevenue != null)
                .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
                .Select(new Projections.TotalRevenue.Chart().Expression)
                .FirstOrDefault();
        }


        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.TotalRevenue != null && i.TotalRevenue> 0)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

           
            var output = data.Select(i => i.TotalRevenue)
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
