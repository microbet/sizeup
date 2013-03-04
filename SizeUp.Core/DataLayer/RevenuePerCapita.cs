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
    public class RevenuePerCapita : Base.Base
    {
        public static PlaceValues<BarChartItem<long?>> Chart(SizeUpContext context, long industryId, long placeId)
        {
            var data = IndustryData.GetMinimumBusinessCount(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<BarChartItem<long?>>
                {
                    City = i.City.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.RevenuePerCapita,
                                Median = null,
                                Name = d.City.Name + ", " + d.City.State.Abbreviation
                            }).FirstOrDefault(),

                    County = i.County.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.RevenuePerCapita,
                                Median = null,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.RevenuePerCapita,
                                Median = null,
                                Name = d.Metro.Name
                            }).FirstOrDefault(),

                    State = i.State.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.RevenuePerCapita,
                                Median = null,
                                Name = d.State.Name
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.RevenuePerCapita,
                                Median = null,
                                Name = "USA"
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentileItem> Percentile(SizeUpContext context, long industryId, long placeId)
        {
            var raw = context.IndustryDataByCities.Where(i => i.IndustryId == industryId && i.Year == Year && i.Quarter == Quarter);

            var data = context.CityCountyMappings
                .Where(i => i.Id == placeId)
                .Select(i => new
                {
                    Place = i,
                    City = raw.Where(d => d.City.CityCountyMappings.Any(c => c.Id == placeId))
                                .Where(d=>i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount),

                    County = raw.Where(d => d.City.CityCountyMappings.Any(c => c.County.CityCountyMappings.Any(co => co.Id == placeId)))
                                .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount),

                    Metro = raw.Where(d => d.City.CityCountyMappings.Any(c => c.County.Metro.Counties.Any(co => co.CityCountyMappings.Any(m => m.Id == placeId))))
                                .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount),

                    State = raw.Where(d => d.City.State.Cities.Any(s => s.CityCountyMappings.Any(c => c.Id == placeId)))
                                .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount),

                    Nation = raw.Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                })
                .Select(i => new
                {
                    County = new
                    {
                        County = i.Place.County,
                        Total = i.County.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Count(),
                        Filtered = i.County.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Where(d => d.RevenuePerCapita <= i.City.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    },
                    Metro = new
                    {
                        Metro = i.Place.County.Metro,
                        Total = i.Metro.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Count(),
                        Filtered = i.Metro.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Where(d => d.RevenuePerCapita <= i.City.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    },
                    State = new
                    {
                        State = i.Place.County.State,
                        Total = i.State.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Count(),
                        Filtered = i.State.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Where(d => d.RevenuePerCapita <= i.City.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    },
                    Nation = new
                    {
                        Total = i.Nation.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Count(),
                        Filtered = i.Nation.Where(d => d.RevenuePerCapita != null && d.RevenuePerCapita > 0).Where(d => d.RevenuePerCapita <= i.City.Select(v => v.RevenuePerCapita).FirstOrDefault()).Count()
                    }
                })
                .Select(i => new PlaceValues<PercentileItem>
                {
                    County = new PercentileItem
                    {
                        Percentile = i.County.Total > 0 ? (int?)(((decimal)i.County.Filtered / (decimal)i.County.Total) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentileItem
                    {
                        Percentile = i.Metro.Total > 0 ? (int?)(((decimal)i.Metro.Filtered / (decimal)i.Metro.Total) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentileItem
                    {
                        Percentile = i.State.Total > 0 ? (int?)(((decimal)i.State.Filtered / (decimal)i.State.Total) * 100) : null,
                        Name = i.State.State.Name
                    },
                    Nation = new PercentileItem
                    {
                        Percentile = i.Nation.Total > 0 ? (int?)(((decimal)i.Nation.Filtered / (decimal)i.Nation.Total) * 100) : null,
                        Name = "USA"
                    }
                }).FirstOrDefault();
            return data;
        }
    }
}
