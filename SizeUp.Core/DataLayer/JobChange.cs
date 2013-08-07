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
        public static JobChangeChartItem Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context, granularity)
                .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount);

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
                .Select(new Projections.JobChange.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, Granularity boundingGranularity)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.County);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.NetJobChange != null);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);

            var value = raw.Where(i => i.GeographicLocationId == place.FirstOrDefault().CountyId).Select(i => i.NetJobChange);

            if (boundingGranularity == Granularity.County)
            {
                raw = raw.Where(i => place.FirstOrDefault().County.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.NetJobChange <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                raw = raw.Where(i => place.FirstOrDefault().County.Metro.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.Metro.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.NetJobChange <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            else if (boundingGranularity == Granularity.State)
            {
                raw = raw.Where(i => place.FirstOrDefault().County.State.GeographicLocation.GeographicLocations.Any(g => g.Id == i.GeographicLocationId));
                output = raw.Select(i => new
                {
                    place.FirstOrDefault().County.State.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.NetJobChange <= value.FirstOrDefault())
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
                    place.FirstOrDefault().County.State.Nation.GeographicLocation.LongName,
                    Total = raw.Count(),
                    Filtered = raw.Count(c => c.NetJobChange <= value.FirstOrDefault())
                })
                .Select(i => new PercentileItem
                {
                    Name = i.LongName,
                    Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
                }).FirstOrDefault();
            }
            return output;
        }
    }
}
