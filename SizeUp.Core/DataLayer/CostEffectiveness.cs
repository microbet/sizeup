using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class CostEffectiveness : Base.Base
    {
        public static BarChartItem<double?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            BarChartItem<double?> output = null;

            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);





            var county = countyData.Select(i => new BarChartItem<double?>
            {
                Value = i.CostEffectiveness,
                Median = null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new BarChartItem<double?>
            {
                Value = i.CostEffectiveness,
                Median = null,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new BarChartItem<double?>
            {
                Value = i.CostEffectiveness,
                Median = null,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new BarChartItem<double?>
            {
                Value = i.CostEffectiveness,
                Median = null,
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

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long placeId, double value, Granularity granularity)
        {
            PercentageItem output = null;

            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.CostEffectiveness != null && i.CostEffectiveness > 0);


            var county = countyData.Select(i => new PercentageItem
            {
                Percentage = i.CostEffectiveness != null ? (int?)(((value - i.CostEffectiveness) / i.CostEffectiveness) * 100) : null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new PercentageItem
            {
                Percentage = i.CostEffectiveness != null ? (int?)(((value - i.CostEffectiveness) / i.CostEffectiveness) * 100) : null,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new PercentageItem
            {
                Percentage = i.CostEffectiveness != null ? (int?)(((value - i.CostEffectiveness) / i.CostEffectiveness) * 100) : null,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new PercentageItem
            {
                Percentage = i.CostEffectiveness != null ? (int?)(((value - i.CostEffectiveness) / i.CostEffectiveness) * 100) : null,
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

        public static List<Band<double>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            IQueryable<double?> values = context.IndustryDataByCounties.Where(i => 0 == 1).Select(i => i.CostEffectiveness);//empty set
            if (granularity == Granularity.County)
            {
                var entities = Base.County.In(context, placeId, boundingGranularity);
                var data = IndustryData.County(context).Where(i => i.IndustryId == industryId);
                values =
                    data.Join(entities, i => i.CountyId, i => i.Id, (d, e) => d)
                    .Select(i => i.CostEffectiveness);
            }
            else if (granularity == Granularity.State)
            {
                var entities = Base.State.In(context, placeId, boundingGranularity);
                var data = IndustryData.State(context).Where(i => i.IndustryId == industryId);
                values =
                    data.Join(entities, i => i.StateId, i => i.Id, (d, e) => d)
                    .Select(i => i.CostEffectiveness);
            }
            var output = values
                .Where(i => i != null && i > 0)
                .ToList()
                .NTile(i => i, bands)
                .Select(i => new Band<double>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            Band<double> old = null;
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
