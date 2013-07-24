using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;



namespace SizeUp.Core.DataLayer
{
    public class YearStarted
    {
        public static List<LineChartItem<int, int>> Chart(SizeUpContext context, long industryId, long placeId, int startYear, int endYear, Granularity granularity)
        {
            var years = Enumerable.Range(startYear, (endYear - startYear) + 1).ToList();
            List<LineChartItem<int, int>> output = null;

            var data = BusinessData.Get(context)
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

            var raw = data.GroupBy(i => i.YearStarted)
                .Select(i => new { Year = i.Key, Count = i.Count() })
                .ToList();

            output = years.GroupJoin(raw.Where(d => d.Year >= startYear && d.Year <= endYear)
               ,i => i, o => o.Year, (i,o)=> new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();

            return output;
        }




        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, int year, Granularity granularity)
        {
            var data = Core.DataLayer.BusinessData.Get(context)
                       .GroupBy(i => new { i.GeographicLocation, i.Industry })
                       .Select(i => new
                       {
                           i.Key.GeographicLocation,
                           i.Key.Industry,
                           Total = i.Count(),
                           Filtered = i.Count(v => v.YearStarted <= year)
                       })
                       .Where(i => i.Industry.Id == industryId)
                       .Where(i => i.Total > 0)
                       .Where(i => i.Total >= CommonFilters.MinimumBusinessCount);

            var place = Core.DataLayer.Place.List(context)
                .Where(i => i.Id == placeId)
                .First();


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

            return data.Select(i => new PercentileItem
            {
                Name = i.GeographicLocation.LongName,
                Percentile = (((decimal)i.Filtered / ((decimal)i.Total + 1) * 100))
            })
                .FirstOrDefault(); 
        }
    }
}
