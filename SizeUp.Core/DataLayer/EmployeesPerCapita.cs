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
    public class EmployeesPerCapita : Base.Base
    {
        public static BarChartItem<double?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            BarChartItem<double?> output = null;

            var cityData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.City.BusinessDataByCities.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);

            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId))
                .Where(i => i.County.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.Metro.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)))
                .Where(i => i.State.BusinessDataByCounties.Where(b => b.IndustryId == i.IndustryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);




            var city = cityData.Select(i => new BarChartItem<double?>
            {
                Value = i.EmployeesPerCapita,
                Median = null,
                Name = i.City.Name + ", " + i.City.State.Abbreviation
            });

            var county = countyData.Select(i => new BarChartItem<double?>
            {
                Value = i.EmployeesPerCapita,
                Median = null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new BarChartItem<double?>
            {
                Value = i.EmployeesPerCapita,
                Median = null,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new BarChartItem<double?>
            {
                Value = i.EmployeesPerCapita,
                Median = null,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new BarChartItem<double?>
            {
                Value = i.EmployeesPerCapita,
                Median = null,
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

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            PercentileItem output = null;
            var currentCity = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(c => c.Id == placeId));


            var countyData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(c => c.County.CityCountyMappings.Any(co => co.Id == placeId)))
                .Where(d => d.EmployeesPerCapita != null && d.EmployeesPerCapita > 0);


            var metroData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.CityCountyMappings.Any(c => c.County.Metro.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))))
                .Where(d => d.EmployeesPerCapita != null && d.EmployeesPerCapita > 0);

            var stateData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.City.State.Cities.Any(s => s.CityCountyMappings.Any(c => c.Id == placeId)))
                .Where(d => d.EmployeesPerCapita != null && d.EmployeesPerCapita > 0);

            var nationData = IndustryData.City(context)
                .Where(i => i.IndustryId == industryId)
                .Where(d => d.EmployeesPerCapita != null && d.EmployeesPerCapita > 0);



           

            var county = countyData.Select(i => new
            {
                County = i.City.CityCountyMappings.Select(c=> c.County).FirstOrDefault(),
                Total = countyData.Count(),
                Filtered = countyData.Where(d => d.EmployeesPerCapita <= currentCity.Select(v=>v.EmployeesPerCapita).FirstOrDefault()).Count()
            })            
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new
            {
                County = i.City.CityCountyMappings.Select(c=> c.County).FirstOrDefault(),
                Total = metroData.Count(),
                Filtered = metroData.Where(d => d.EmployeesPerCapita <= currentCity.Select(v=>v.EmployeesPerCapita).FirstOrDefault()).Count()
            })            
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.County.Metro.Name
            });

            var state = stateData.Select(i => new
            {
                County = i.City.CityCountyMappings.Select(c=> c.County).FirstOrDefault(),
                Total = stateData.Count(),
                Filtered = stateData.Where(d => d.EmployeesPerCapita <= currentCity.Select(v=>v.EmployeesPerCapita).FirstOrDefault()).Count()
            })            
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
                Name = i.County.State.Name
            });

            var nation = nationData.Select(i => new
            {
                County = i.City.CityCountyMappings.Select(c=> c.County).FirstOrDefault(),
                Total = nationData.Count(),
                Filtered = nationData.Where(d => d.EmployeesPerCapita <= currentCity.Select(v=>v.EmployeesPerCapita).FirstOrDefault()).Count()
            })            
            .Select(i => new PercentileItem
            {
                Percentile = i.Total > 0 ? (int?)(((decimal)i.Filtered / (decimal)i.Total) * 100) : null,
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
            IQueryable<double?> values = context.IndustryDataByZips.Where(i => 0 == 1).Select(i => i.EmployeesPerCapita);//empty set
            if (granularity == Granularity.ZipCode)
            {
                var entities = Base.ZipCode.In(context, placeId, boundingGranularity);
                var data = IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                values =
                    data.Join(entities, i => i.ZipCodeId, i => i.Id, (d, e) => d)
                    .Select(i => i.EmployeesPerCapita);
            }
            else if (granularity == Granularity.County)
            {
                var entities = Base.County.In(context, placeId, boundingGranularity);
                var data = IndustryData.County(context).Where(i => i.IndustryId == industryId);
                values =
                    data.Join(entities, i => i.CountyId, i => i.Id, (d, e) => d)
                    .Select(i => i.EmployeesPerCapita);
            }
            else if (granularity == Granularity.State)
            {
                var entities = Base.State.In(context, placeId, boundingGranularity);
                var data = IndustryData.State(context).Where(i => i.IndustryId == industryId);
                values =
                    data.Join(entities, i => i.StateId, i => i.Id, (d, e) => d)
                    .Select(i => i.EmployeesPerCapita);
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
