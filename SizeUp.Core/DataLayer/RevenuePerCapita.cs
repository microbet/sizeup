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
    public class RevenuePerCapita
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context, granularity)
                .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
               .Where(i => i.RevenuePerCapita != null);

            var place = Core.DataLayer.Place.List(context)
                .Where(i => i.Id == placeId)
                .FirstOrDefault();


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocationId == place.City.Id);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocationId == place.County.Id);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocationId == place.Metro.Id);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocationId == place.State.Id);
            }
            else if (granularity == Granularity.Nation)
            {
                data = data.Where(i => i.GeographicLocationId == place.Nation.Id);
            }
            return data
                .Select(new Projections.RevenuePerCapita.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, Granularity boundingGranularity)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.City);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);

            var value = raw.Where(i => i.GeographicLocationId == place.FirstOrDefault().CityId).Select(i => i.RevenuePerCapita);

            if (boundingGranularity == Granularity.County)
            {
                var geo = place.Select(i => new KeyValue<long?, string> { Key = i.CountyId, Value = i.County.GeographicLocation.LongName }).FirstOrDefault();
                raw = raw.Where(i => place.FirstOrDefault().County.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.RevenuePerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                var geo = place.Select(i => new KeyValue<long?, string> { Key = i.County.MetroId, Value = i.County.Metro.GeographicLocation.LongName }).FirstOrDefault();
                raw = raw.Where(i => place.FirstOrDefault().County.Metro.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.Metro.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.RevenuePerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.State)
            {
                var geo = place.Select(i => new KeyValue<long?, string> { Key = i.County.StateId, Value = i.County.State.GeographicLocation.LongName }).FirstOrDefault();
                raw = raw.Where(i => place.FirstOrDefault().County.State.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.State.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.RevenuePerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                var geo = place.Select(i => new KeyValue<long?, string> { Key = i.County.State.NationId, Value = i.County.State.Nation.GeographicLocation.LongName }).FirstOrDefault();
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.State.Nation.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.RevenuePerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            return output;
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                .Where(i => i.IndustryId == industryId);

            var output = data.Select(i => i.RevenuePerCapita)
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
