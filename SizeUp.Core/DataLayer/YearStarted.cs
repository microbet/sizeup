using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;


namespace SizeUp.Core.DataLayer
{
    public class YearStarted : Base.Base
    {
        public static List<LineChartItem<int, int>> Chart(SizeUpContext context, long industryId, long placeId, int startYear, int endYear, Granularity granularity)
        {
            var years = Enumerable.Range(startYear, (endYear - startYear) + 1).ToList();

            List<LineChartItem<int, int>> output = null;

            var cityData = BusinessData.City(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId))
                .Select(d => new
                {
                    City = d.City,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);

            var countyData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Select(d => new
                {
                    County = d.County,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var metroData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Select(d => new
                {
                    Metro = d.Metro,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var stateData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Select(d => new
                {
                    State = d.State,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var nationData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Select(d => new
                {
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);




            var city = cityData
            .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear)
            .GroupBy(d => d.YearStarted)
            .Select(d => new { Year = d.Key.Value, Count = d.Count() });

            var county = countyData
            .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear)
            .GroupBy(d => d.YearStarted)
            .Select(d => new { Year = d.Key.Value, Count = d.Count() });

            var metro = metroData
            .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear)
            .GroupBy(d => d.YearStarted)
            .Select(d => new { Year = d.Key.Value, Count = d.Count() });


            var state = stateData
            .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear)
            .GroupBy(d => d.YearStarted)
            .Select(d => new { Year = d.Key.Value, Count = d.Count() });


            var nation = nationData
            .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear)
            .GroupBy(d => d.YearStarted)
            .Select(d => new { Year = d.Key.Value, Count = d.Count() });


            if (granularity == Granularity.City)
            {
                output = years.GroupJoin(city.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Select(v=>v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();
            }
            else if (granularity == Granularity.County)
            {
                output = years.GroupJoin(county.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();
            }
            else if (granularity == Granularity.Metro)
            {
                output = years.GroupJoin(metro.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();
            }
            else if (granularity == Granularity.State)
            {
                output = years.GroupJoin(state.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();
            }
            else if (granularity == Granularity.Nation)
            {
                output = years.GroupJoin(nation.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Select(v => v.Count).DefaultIfEmpty(0).FirstOrDefault() }).ToList();
            }


            return output;
        }




        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, int year, Granularity granularity)
        {
            PercentileItem output = null;
            var cityData = BusinessData.City(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId))
                .Select(d => new
                {
                    City = d.City,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);

            var countyData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Select(d => new
                {
                    County = d.County,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var metroData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Select(d => new
                {
                    Metro = d.Metro,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var stateData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Select(d => new
                {
                    State = d.State,
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);


            var nationData = BusinessData.County(context)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Select(d => new
                {
                    YearStarted = d.YearEstablished ?? d.YearAppeared
                })
                .Where(d => d.YearStarted != null);




            var city = cityData.Select(d => new
            {
                City = d.City,
                Total = cityData.Count(),
                Filtered = cityData.Where(f => f.YearStarted >= year).Count()
            })
            .Select(i=> new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.City.Name + ", " + i.City.State.Abbreviation
            });

            var county = countyData.Select(d => new
            {
                County = d.County,
                Total = countyData.Count(),
                Filtered = countyData.Where(f => f.YearStarted >= year).Count()
            })
            .Select(i=> new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(d => new
            {
                Metro = d.Metro,
                Total = metroData.Count(),
                Filtered = metroData.Where(f => f.YearStarted >= year).Count()
            })
            .Select(i=> new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.Metro.Name
            });


            var state = stateData.Select(d => new
            {
                State = d.State,
                Total = stateData.Count(),
                Filtered = stateData.Where(f => f.YearStarted >= year).Count()
            })
            .Select(i=> new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.State.Name
            });


            var nation = nationData.Select(d => new
            {
                Total = nationData.Count(),
                Filtered = nationData.Where(f => f.YearStarted >= year).Count()
            })
            .Select(i=> new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = "USA"
            });


            if (granularity == Granularity.City)
            {
                output = city.FirstOrDefault();
            }
            else if (granularity == Granularity.County)
            {
                output = county.FirstOrDefault();
            }
            else if (granularity == Granularity.Metro)
            {
                output = metro.FirstOrDefault();
            }
            else if (granularity == Granularity.State)
            {
                output = state.FirstOrDefault();
            }
            else if (granularity == Granularity.Nation)
            {
                output = nation.FirstOrDefault();
            }


            return output;
        }
    }
}
