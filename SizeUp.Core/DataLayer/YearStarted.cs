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
        public static PlaceValues<List<LineChartItem<int, int>>> Chart(SizeUpContext context, long industryId, long placeId, int startYear, int endYear)
        {
            var years = Enumerable.Range(startYear, (endYear - startYear) + 1).ToList();
            var data = BusinessData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new
                {
                    City = i.City.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear && d.YearStarted != null)
                    .Select(d => d.YearStarted)
                    .GroupBy(d => d)
                    .Select(d => new { Year = d.Key.Value, Count = d.Count() }),

                    County = i.County.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear && d.YearStarted != null)
                    .Select(d => d.YearStarted)
                    .GroupBy(d => d)
                    .Select(d => new { Year = d.Key.Value, Count = d.Count() }),

                    Metro = i.Metro.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear && d.YearStarted != null)
                    .Select(d => d.YearStarted)
                    .GroupBy(d => d)
                    .Select(d => new { Year = d.Key.Value, Count = d.Count() }),

                    State = i.State.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear && d.YearStarted != null)
                    .Select(d => d.YearStarted)
                    .GroupBy(d => d)
                    .Select(d => new { Year = d.Key.Value, Count = d.Count() }),

                    Nation = i.Nation.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted >= startYear && d.YearStarted <= endYear && d.YearStarted != null)
                    .Select(d => d.YearStarted)
                    .GroupBy(d => d)
                    .Select(d => new { Year = d.Key.Value, Count = d.Count() })
                }).FirstOrDefault();

            var obj = new PlaceValues<List<LineChartItem<int, int>>>()
            {
                City = years.Join(data.City.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Count }).ToList(),
                County = years.Join(data.County.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Count }).ToList(),
                Metro = years.Join(data.Metro.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Count }).ToList(),
                State = years.Join(data.State.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Count }).ToList(),
                Nation = years.Join(data.Nation.ToList(), o => o, i => i.Year, (i, o) => new LineChartItem<int, int>() { Key = i, Value = o.Count }).ToList()
            };
            return obj;
        }

        public static PlaceValues<int> Count(SizeUpContext context, long industryId, long placeId, int year)
        {
            var data = BusinessData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<int>
                {
                    City = i.City.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted == year)
                    .Count(),

                    County = i.County.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted == year)
                    .Count(),

                    Metro = i.Metro.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted == year)
                    .Count(),

                    State = i.State.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted == year)
                    .Count(),

                    Nation = i.Nation.Select(d => new
                    {
                        YearStarted = d.YearEstablished ?? d.YearAppeared
                    })
                    .Where(d => d.YearStarted == year)
                    .Count()
                }).FirstOrDefault();
            return data;
        }


        public static PlaceValues<PercentileItem> Percentile(SizeUpContext context, long industryId, long placeId, int year)
        {
            var data = BusinessData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new
                {
                    Place = i.Place,
                    City = i.City.Select(d => d.YearEstablished ?? d.YearAppeared).Where(d => d != null),

                    County = i.County.Select(d => d.YearEstablished ?? d.YearAppeared).Where(d => d != null),

                    Metro = i.Metro.Select(d => d.YearEstablished ?? d.YearAppeared).Where(d => d != null),

                    State = i.State.Select(d => d.YearEstablished ?? d.YearAppeared).Where(d => d != null),

                    Nation = i.Nation.Select(d => d.YearEstablished ?? d.YearAppeared).Where(d => d != null)

                })
                .Select(i => new
                {
                    City = i.City.Select(d => new
                    {
                        City = i.Place.City,
                        Total = i.City.Count(),
                        Filtered = i.City.Where(f => f >= year).Count()
                    }).FirstOrDefault(),
                    County = i.County.Select(d => new
                    {
                        County = i.Place.County,
                        Total = i.County.Count(),
                        Filtered = i.County.Where(f => f >= year).Count()
                    }).FirstOrDefault(),
                    Metro = i.Metro.Select(d => new
                    {
                        Metro = i.Place.County.Metro,
                        Total = i.Metro.Count(),
                        Filtered = i.Metro.Where(f => f >= year).Count()
                    }).FirstOrDefault(),
                    State = i.State.Select(d => new
                    {
                        State = i.Place.County.State,
                        Total = i.State.Count(),
                        Filtered = i.State.Where(f => f >= year).Count()
                    }).FirstOrDefault(),
                    Nation = i.Nation.Select(d => new
                    {
                        Total = i.Nation.Count(),
                        Filtered = i.Nation.Where(f => f >= year).Count()
                    }).FirstOrDefault()
                })
                .Select(i => new PlaceValues<PercentileItem>
                {
                    City = new PercentileItem
                    {
                        Percentile = i.City.Total > MinimumBusinessCount ? (int?)(((decimal)i.City.Filtered / (decimal)i.City.Total) * 100) : null,
                        Name = i.City.City.Name + ", " + i.City.City.State.Abbreviation
                    },
                    County = new PercentileItem
                    {
                        Percentile = i.County.Total > MinimumBusinessCount ? (int?)(((decimal)i.County.Filtered / (decimal)i.County.Total) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentileItem
                    {
                        Percentile = i.Metro.Total > MinimumBusinessCount ? (int?)(((decimal)i.Metro.Filtered / (decimal)i.Metro.Total) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentileItem
                    {
                        Percentile = i.State.Total > MinimumBusinessCount ? (int?)(((decimal)i.State.Filtered / (decimal)i.State.Total) * 100) : null,
                        Name = i.State.State.Name
                    },
                    Nation = new PercentileItem
                    {
                        Percentile = i.Nation.Total > MinimumBusinessCount ? (int?)(((decimal)i.Nation.Filtered / (decimal)i.Nation.Total) * 100) : null,
                        Name = "USA"
                    }
                }).FirstOrDefault();


            return data;
        }


    }
}
