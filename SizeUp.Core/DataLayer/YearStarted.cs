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
        public static List<LineChartItem<int, int>> Chart(SizeUpContext context, long industryId, long geographicLocationId, int startYear, int endYear )
        {
            var years = Enumerable.Range(startYear, (endYear - startYear) + 1).ToList();
            List<LineChartItem<int, int>> output = null;

            var data = Core.DataLayer.BusinessData.Get(context)
               .Where(i => i.IndustryId == industryId)
               .Where(i => i.GeographicLocationId == geographicLocationId);

            var raw = data.GroupBy(i => i.YearStarted)
                .Select(i => new { Year = i.Key, Count = i.Count() })
                .ToList();

            output = years.GroupJoin(raw.Where(d => d.Year >= startYear && d.Year <= endYear)
               ,i => i, o => o.Year, (i,o)=> new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();

            return output;
        }




        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, int year)
        {
            PercentileItem output = null;

            var raw = Core.DataLayer.BusinessData.Get(context)
               .Where(i => i.IndustryId == industryId)
               .Where(i => i.GeographicLocationId == geographicLocationId)
               .Where(i => i.YearStarted != null);

            output = raw.Select(i => new PercentileItem
            {
                Name = i.GeographicLocation.LongName,
                Percentile = (((decimal)raw.Count(c => c.YearStarted >= year) / ((decimal)raw.Count()) * 100))
            }).FirstOrDefault();

            return output;
        }
    }
}
