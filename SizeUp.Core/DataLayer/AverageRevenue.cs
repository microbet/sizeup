using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer
{
    public class AverageRevenue : Base
    {
        public static /*IQueryable<PlaceValues<BarChartItem<long?>>>*/ object Chart(SizeUpContext context, long industryId, long placeId)
        {
            /*
            var data = context.CityCountyMappings
                .Where(i => i.Id == placeId)
                .Select(i => new PlaceValues<BarChartItem<long?>>
                {
                    City = i.City.IndustryDataByCities
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => i.City.BusinessDataByCities.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.City.Name + ", " + i.City.State.Abbreviation
                        }).FirstOrDefault(),

                    County = i.County.IndustryDataByCounties
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => i.County.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.County.Name + ", " + i.City.State.Abbreviation
                        }).FirstOrDefault(),

                    Metro = i.County.Metro.IndustryDataByMetroes
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => i.County.Metro.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.County.Metro.Name
                        }).FirstOrDefault(),

                    State = i.County.State.IndustryDataByStates
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.City.State.Name
                        }).FirstOrDefault(),

                    Nation = context.IndustryDataByNations
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = d.MedianRevenue,
                            Name = "USA"
                        }).FirstOrDefault()
                });
             * */

            var data = IndustryData.Get(context, industryId)
                .Where(i=>i.Place.Id == placeId)
                .Select(i=> new PlaceValues<BarChartItem<long?>>
                {
                    City = i.City.Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Select(d=> new BarChartItem<long?>
                            {
                                Value = d.AverageRevenue,
                                Median = null,
                                Name = d.City.Name + ", " + d.City.State.Abbreviation
                            }).FirstOrDefault(),

                    County = i.County.Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                            .Select(d=> new BarChartItem<long?>
                            {
                                Value = d.AverageRevenue,
                                Median = null,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),
                          

                       

           

                    Metro = i.Metro.Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                    
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.Metro.
                        }).FirstOrDefault(),

                    State = i.County.State.IndustryDataByStates
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => i.County.State.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = null,
                            Name = i.City.State.Name
                        }).FirstOrDefault(),

                    Nation = context.IndustryDataByNations
                        .Where(d => d.IndustryId == industryId)
                        .Where(d => d.AverageRevenue != null && d.AverageRevenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                        .Where(d => context.BusinessDataByCounties.Where(b => b.IndustryId == industryId && b.Business.IsActive).Count() >= MinimumBusinessCount)
                        .Select(d => new BarChartItem<long?>
                        {
                            Value = d.AverageRevenue,
                            Median = d.MedianRevenue,
                            Name = "USA"
                        }).FirstOrDefault()*/
            return data;
        }

        public static IQueryable<PlaceValues<PercentileItem>> Percentile(SizeUpContext context, long industryId, long placeId, long value)
        {
            var data = context.CityCountyMappings
                .Where(i => i.Id == placeId)
                .Select(i => new
                {
                    City = i.City.BusinessDataByCities
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d => d.Revenue != null && d.Revenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    County = i.County.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d => d.Revenue != null && d.Revenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),

                    Metro = i.County.Metro.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d => d.Revenue != null && d.Revenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),


                    State = i.County.State.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d => d.Revenue != null && d.Revenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter),


                    Nation = context.BusinessDataByCounties
                        .Where(d => d.IndustryId == industryId && d.Business.IsActive)
                        .Where(d => d.Revenue != null && d.Revenue > 0)
                        .Where(d => d.Year == Year && d.Quarter == Quarter)
                })
                .Select(i => new
                {
                    City = new
                    {
                        City = i.City.FirstOrDefault().City,
                        Total = i.City.Count(),
                        Filtered = i.City.OrderBy(d => d.Revenue).Where(d => d.Revenue <= value).Count()
                    },
                    County = new
                    {
                        County = i.County.FirstOrDefault().County,
                        Total = i.County.Count(),
                        Filtered = i.County.OrderBy(d => d.Revenue).Where(d => d.Revenue <= value).Count()
                    },
                    Metro = new
                    {
                        Metro = i.Metro.FirstOrDefault().Metro,
                        Total = i.Metro.Count(),
                        Filtered = i.Metro.OrderBy(d => d.Revenue).Where(d => d.Revenue <= value).Count()
                    },
                    State = new
                    {
                        State = i.State.FirstOrDefault().State,
                        Total = i.State.Count(),
                        Filtered = i.State.OrderBy(d => d.Revenue).Where(d => d.Revenue <= value).Count()
                    },
                    Nation = new
                    {
                        Total = i.Nation.Count(),
                        Filtered = i.Nation.OrderBy(d => d.Revenue).Where(d => d.Revenue <= value).Count()
                    }
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
                });
                


                    
            return data;
        }
    }
}
