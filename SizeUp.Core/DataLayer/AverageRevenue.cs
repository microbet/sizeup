using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class AverageRevenue : Base.Base
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            BarChartItem<long?> output = null;

            var cityData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.City.BusinessDataByCities.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);

            var countyData = IndustryData.County(context)            
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.County.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.Metro.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.State.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);




            var city = cityData.Select(i => new BarChartItem<long?>
            {
                Value = i.AverageRevenue,
                Median = null,
                Name = i.City.Name + ", " + i.City.State.Abbreviation
            });

            var county = countyData.Select(i => new BarChartItem<long?>
            {
                Value = i.AverageRevenue,
                Median = null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new BarChartItem<long?>
            {
                Value = i.AverageRevenue,
                Median = null,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new BarChartItem<long?>
            {
                Value = i.AverageRevenue,
                Median = null,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new BarChartItem<long?>
            {
                Value = i.AverageRevenue,
                Median = i.MedianRevenue,
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

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, long value, Granularity granularity)
        {
            PercentileItem output = null;

            var cityData = BusinessData.City(context)
                .Where(i => i.Revenue != null && i.Revenue > 0)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId));

            var countyData = BusinessData.County(context)
                .Where(i => i.Revenue != null && i.Revenue > 0)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId));

            var metroData = BusinessData.County(context)
                .Where(i => i.Revenue != null && i.Revenue > 0)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));

            var stateData = BusinessData.County(context)
                .Where(i => i.Revenue != null && i.Revenue > 0)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId)
                .Where(i => i.County.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));


            var nationData = BusinessData.County(context)
                .Where(i => i.Revenue != null && i.Revenue > 0)
                .Where(i => i.IndustryId == industryId && i.Business.IndustryId == industryId);

            var city = cityData.Select(d => new
            {
                City = d.City,
                Total = cityData.Count(),
                Filtered = cityData.Where(v => v.Revenue <= value).Count()
            })
            .Where(d => cityData.Count() >= MinimumBusinessCount)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.City.Name + ", " + i.City.State.Abbreviation
            });


            var county = countyData.Select(d => new
            {
                County = d.County,
                Total = countyData.Count(),
                Filtered = countyData.Where(v => v.Revenue <= value).Count()
            })
            .Where(d => countyData.Count() >= MinimumBusinessCount)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });


            var metro = metroData.Select(d => new
            {
                Metro = d.County.Metro,
                Total = metroData.Count(),
                Filtered = metroData.Where(v => v.Revenue <= value).Count()
            })
            .Where(d => metroData.Count() >= MinimumBusinessCount)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.Metro.Name
            });


            var state = stateData.Select(d => new
            {
                State = d.County.State,
                Total = stateData.Count(),
                Filtered = stateData.Where(v => v.Revenue <= value).Count()
            })
            .Where(d => stateData.Count() >= MinimumBusinessCount)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.State.Name
            });



            var nation = nationData.Select(d => new
            {
                Total = nationData.Count(),
                Filtered = nationData.Where(v => v.Revenue <= value).Count()
            })
            .Where(d => nationData.Count() >= MinimumBusinessCount)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
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

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            IQueryable<long?> averageRevenue = context.IndustryDataByZips.Where(i => 0 == 1).Select(i => i.AverageRevenue);//empty set
            if (granularity == Granularity.ZipCode)
            {
                var entities = ZipCode.In(context, placeId, boundingGranularity);
                var data = IndustryData.ZipCode(context).Where(i=>i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.ZipCodeId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            else if (granularity == Granularity.County)
            {
                var entities = County.In(context, placeId, boundingGranularity);
                var data = IndustryData.County(context).Where(i => i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.CountyId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            else if (granularity == Granularity.State)
            {
                var entities = State.In(context, placeId, boundingGranularity);
                var data = IndustryData.State(context).Where(i => i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.StateId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            var output = averageRevenue
                .Where(i=>i!= null && i > 0)
                .ToList()
                .NTile(i=>i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            Band<long> old = null;
            foreach (var band in output)
            {
                if (old != null)
                {
                    old.Max = band.Min;
                }
                old = band;
            }
            return output;
        }
    }
}
