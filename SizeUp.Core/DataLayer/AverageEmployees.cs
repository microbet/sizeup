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
    public class AverageEmployees : Base.Base
    {
        public static PlaceValues<BarChartItem<long?>> Chart(SizeUpContext context, long industryId, long placeId)
        {
            var data = IndustryData.GetMinimumBusinessCount(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<BarChartItem<long?>>
                {
                    City = i.City.Where(d => d.AverageEmployees != null && d.AverageEmployees > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageEmployees,
                                Median = null,
                                Name = d.City.Name + ", " + d.City.State.Abbreviation
                            }).FirstOrDefault(),

                    County = i.County.Where(d => d.AverageEmployees != null && d.AverageEmployees > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageEmployees,
                                Median = null,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(d => d.AverageEmployees != null && d.AverageEmployees > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageEmployees,
                                Median = null,
                                Name = d.Metro.Name
                            }).FirstOrDefault(),

                    State = i.State.Where(d => d.AverageEmployees != null && d.AverageEmployees > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageEmployees,
                                Median = null,
                                Name = d.State.Name
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(d => d.AverageEmployees != null && d.AverageEmployees > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageEmployees,
                                Median = d.MedianEmployees,
                                Name = "USA"
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentileItem> Percentile(SizeUpContext context, long industryId, long placeId, long value)
        {

            var data = BusinessData.GetMinimumBusinessCount(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new
                {

                    City = i.City.Select(d => new
                            {
                                City = d.City,
                                Total = i.City.Where(v => v.Employees != null && v.Employees > 0).Count(),
                                Filtered = i.City.Where(v => v.Employees != null && v.Employees > 0).OrderBy(v => v.Employees).Where(v => v.Employees <= value).Count()
                            }).FirstOrDefault(),

                    County = i.County.Where(d => d.Employees != null && d.Employees > 0)
                            .Select(d => new
                            {
                                County = d.County,
                                Total = i.County.Where(v => v.Employees != null && v.Employees > 0).Count(),
                                Filtered = i.County.Where(v => v.Employees != null && v.Employees > 0).OrderBy(v => v.Employees).Where(v => v.Employees <= value).Count()
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(d => d.Employees != null && d.Employees > 0)
                            .Select(d => new
                            {
                                Metro = d.Metro,
                                Total = i.Metro.Where(v => v.Employees != null && v.Employees > 0).Count(),
                                Filtered = i.Metro.Where(v => v.Employees != null && v.Employees > 0).OrderBy(v => v.Employees).Where(v => v.Employees <= value).Count()
                            }).FirstOrDefault(),

                    State = i.State.Where(d => d.Employees != null && d.Employees > 0)
                            .Select(d => new
                            {
                                State = d.State,
                                Total = i.State.Where(v => v.Employees != null && v.Employees > 0).Count(),
                                Filtered = i.State.Where(v => v.Employees != null && v.Employees > 0).OrderBy(v => v.Employees).Where(v => v.Employees <= value).Count()
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(d => d.Employees != null && d.Employees > 0)
                            .Select(d => new
                            {
                                Total = i.Nation.Where(v => v.Employees != null && v.Employees > 0).Count(),
                                Filtered = i.Nation.Where(v => v.Employees != null && v.Employees > 0).OrderBy(v => v.Employees).Where(v => v.Employees <= value).Count()
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
