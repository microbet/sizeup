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
    public class Turnover : Base.Base
    {
        public static TurnoverChartItem Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            TurnoverChartItem output = null;


            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId));

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId);




            var county = countyData.Select(i => new TurnoverChartItem
            {
                Hires = i.Hires,
                Turnover = i.TurnoverRate * 100,
                Separations = i.Separations,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new TurnoverChartItem
            {
                Hires = i.Hires,
                Turnover = i.TurnoverRate * 100,
                Separations = i.Separations,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new TurnoverChartItem
            {
                Hires = i.Hires,
                Turnover = i.TurnoverRate * 100,
                Separations = i.Separations,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new TurnoverChartItem
            {
                Hires = i.Hires,
                Turnover = i.TurnoverRate * 100,
                Separations = i.Separations,
                Name = "USA"
            });

            if (granularity == Granularity.County)
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

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            PercentileItem output = null;
            var currentCounty = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(c => c.Id == placeId));


            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId);


            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId);

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId);

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId);



            var county = countyData.Select(i => new
            {
                County = currentCounty.Select(c=>c.County).FirstOrDefault(),
                Total = countyData.Count(),
                Filtered = countyData.Where(d => d.TurnoverRate >= currentCounty.Select(v => v.TurnoverRate).FirstOrDefault()).Count()
            })
            .Where(d => d.Total >= MinimumBusinessCount)
            .Where(i => currentCounty.Select(v => v.TurnoverRate).FirstOrDefault() != null)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new
            {
                County = currentCounty.Select(c => c.County).FirstOrDefault(),
                Total = metroData.Count(),
                Filtered = metroData.Where(d => d.TurnoverRate >= currentCounty.Select(v => v.TurnoverRate).FirstOrDefault()).Count()
            })
            .Where(d => d.Total >= MinimumBusinessCount)
            .Where(i => currentCounty.Select(v => v.TurnoverRate).FirstOrDefault() != null)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.County.Metro.Name
            });

            var state = stateData.Select(i => new
            {
                County = currentCounty.Select(c => c.County).FirstOrDefault(),
                Total = stateData.Count(),
                Filtered = stateData.Where(d => d.TurnoverRate >= currentCounty.Select(v => v.TurnoverRate).FirstOrDefault()).Count()
            })
            .Where(d => d.Total >= MinimumBusinessCount)
            .Where(i => currentCounty.Select(v => v.TurnoverRate).FirstOrDefault() != null)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = i.County.State.Name
            });

            var nation = nationData.Select(i => new
            {
                Total = nationData.Count(),
                Filtered = nationData.Where(d => d.TurnoverRate >= currentCounty.Select(v => v.TurnoverRate).FirstOrDefault()).Count()
            })
            .Where(d => d.Total >= MinimumBusinessCount)
            .Where(i => currentCounty.Select(v => v.TurnoverRate).FirstOrDefault() != null)
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : 100,
                Name = "USA"
            });

            if (granularity == Granularity.County)
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
