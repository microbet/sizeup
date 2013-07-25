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
            PercentileItem output = null;

            var raw = Core.DataLayer.BusinessData.Get(context)
                .Where(i => i.IndustryId == industryId);

            KeyValue<long?, string> geo = new KeyValue<long?,string>();
            if (granularity == Granularity.City)
            {
               geo = Core.DataLayer.Place.Get(context)
                    .Where(i => i.Id == placeId)
                    .Select(i => new KeyValue<long?, string>
                    {
                        Key = i.CityId,
                        Value = i.City.GeographicLocation.LongName
                    })
                    .FirstOrDefault();
            }
            else if (granularity == Granularity.County)
            {
                geo = Core.DataLayer.Place.Get(context)
                     .Where(i => i.Id == placeId)
                     .Select(i => new KeyValue<long?, string>
                     {
                         Key = i.CountyId,
                         Value = i.County.GeographicLocation.LongName
                     })
                     .FirstOrDefault();
            }
            else if (granularity == Granularity.Metro)
            {
                geo = Core.DataLayer.Place.Get(context)
                     .Where(i => i.Id == placeId)
                     .Select(i => new KeyValue<long?, string>
                     {
                         Key = i.County.MetroId,
                         Value = i.County.Metro.GeographicLocation.LongName
                     })
                     .FirstOrDefault();
            }
            else if (granularity == Granularity.State)
            {
                geo = Core.DataLayer.Place.Get(context)
                      .Where(i => i.Id == placeId)
                      .Select(i => new KeyValue<long?, string>
                      {
                          Key = i.County.StateId,
                          Value = i.County.State.GeographicLocation.LongName
                      })
                      .FirstOrDefault();
            }
            else if (granularity == Granularity.Nation)
            {
                geo = Core.DataLayer.Place.Get(context)
                     .Where(i => i.Id == placeId)
                     .Select(i => new KeyValue<long?, string>
                     {
                         Key = i.County.State.NationId,
                         Value = i.County.State.Nation.GeographicLocation.LongName
                     })
                     .FirstOrDefault();
            }



            raw = raw.Where(i => i.GeographicLocationId == geo.Key);
            output = new PercentileItem
            {
                Name = geo.Value,
                Percentile = (((decimal)raw.Count(c => c.YearStarted <= year) / ((decimal)raw.Count()) * 100))
            };

            return output;
        }
    }
}
