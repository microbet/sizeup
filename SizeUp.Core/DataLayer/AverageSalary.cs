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
    public class AverageSalary : Base.Base
    {
        public static PlaceValues<BarChartItem<long?>> Chart(SizeUpContext context, long industryId, long placeId)
        {
            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<BarChartItem<long?>>
                {
                    County = i.County.Where(d => d.AverageAnnualSalary != null && d.AverageAnnualSalary > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageAnnualSalary,
                                Median = null,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(d => d.AverageAnnualSalary != null && d.AverageAnnualSalary > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageAnnualSalary,
                                Median = null,
                                Name = d.Metro.Name
                            }).FirstOrDefault(),

                    State = i.State.Where(d => d.AverageAnnualSalary != null && d.AverageAnnualSalary > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageAnnualSalary,
                                Median = null,
                                Name = d.State.Name
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(d => d.AverageAnnualSalary != null && d.AverageAnnualSalary > 0)
                            .Select(d => new BarChartItem<long?>
                            {
                                Value = d.AverageAnnualSalary,
                                Median = null,
                                Name = "USA"
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentageItem> Percentage(SizeUpContext context, long industryId, long placeId, long value)
        {
            decimal salary = (decimal)value;
            var data = IndustryData.Get(context, industryId)
                .Where(i=>i.Place.Id == placeId)
                .Select(i => new
                {
                    County = i.County.Where(v => v.AverageAnnualSalary != null && v.AverageAnnualSalary > 0)
                            .Select(d => new
                            {
                                County = d.County,
                                Value = d.AverageAnnualSalary
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(v => v.AverageAnnualSalary != null && v.AverageAnnualSalary > 0)
                            .Select(d => new
                            {
                                Metro = d.Metro,
                                Value = d.AverageAnnualSalary
                            }).FirstOrDefault(),

                    State = i.State.Where(v => v.AverageAnnualSalary != null && v.AverageAnnualSalary > 0)
                            .Select(d => new
                            {
                                State = d.State,
                                Value = d.AverageAnnualSalary
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(v => v.AverageAnnualSalary != null && v.AverageAnnualSalary > 0)
                            .Select(d => new
                            {
                                Value = d.AverageAnnualSalary
                            }).FirstOrDefault()
                })
                .Select(i => new PlaceValues<PercentageItem>
                {
                    County = new PercentageItem
                    {
                        Percentage = i.County.Value != null ? (int?)(((salary - i.County.Value) / i.County.Value) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentageItem
                    {
                        Percentage = i.Metro.Value != null ? (int?)(((salary - i.Metro.Value) / i.Metro.Value) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentageItem
                    {
                        Percentage = i.State.Value != null ? (int?)(((salary - i.State.Value) / i.State.Value) * 100) : null,
                        Name = i.State.State.Name
                    },
                    Nation = new PercentageItem
                    {
                        Percentage = i.Nation.Value != null ? (int?)(((salary - i.Nation.Value) / i.Nation.Value) * 100) : null,
                        Name = "USA"
                    }
                }).FirstOrDefault();

            return data;
        }
    }
}
