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
        public static BarChartItem<double?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.GetMinimumBusinessCount(context, granularity)
                 .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.List(context)
               .Where(i => i.Id == placeId)
               .FirstOrDefault();


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.City.Id);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.County.Id);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Metro.Id);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.State.Id);
            }
            else if (granularity == Granularity.Nation)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Nation.Id);
            }
            return data
                .Select(new Projections.EmployeesPerCapita.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, Granularity boundingGranularity)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.City);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId)
                .FirstOrDefault();

            var value = raw.Where(i=>i.GeographicLocationId == place.CityId).Select(i=>i.EmployeesPerCapita);

            if (boundingGranularity == Granularity.County)
            {
                raw = raw.Where(i => place.County.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.County.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.EmployeesPerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                raw = raw.Where(i => place.County.Metro.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.County.Metro.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.EmployeesPerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.State)
            {
                raw = raw.Where(i => place.County.State.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.County.State.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.EmployeesPerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                output = raw.Select(i => new
                {
                    place.County.State.Nation.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.EmployeesPerCapita <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            return output;
        }

        public static List<Band<double>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var data = Core.DataLayer.IndustryData.GetMinimumBusinessCount(context, granularity, placeId, boundingGranularity)
                .Where(i => i.IndustryId == industryId);

            var output = data.Select(i => i.EmployeesPerCapita)
                .Where(i => i != null)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<double>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
