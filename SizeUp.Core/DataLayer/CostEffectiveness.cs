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
    public class CostEffectiveness : Base.Base
    {
        public static PlaceValues<BarChartItem<double?>> Chart(SizeUpContext context, long industryId, long placeId)
        {
            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => new PlaceValues<BarChartItem<double?>>
                {
                    County = i.County.Where(d => d.CostEffectiveness != null && d.CostEffectiveness > 0)
                            .Select(d => new BarChartItem<double?>
                            {
                                Value = d.CostEffectiveness,
                                Median = null,
                                Name = d.County.Name + ", " + d.County.State.Abbreviation
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(d => d.CostEffectiveness != null && d.CostEffectiveness > 0)
                            .Select(d => new BarChartItem<double?>
                            {
                                Value = d.CostEffectiveness,
                                Median = null,
                                Name = d.Metro.Name
                            }).FirstOrDefault(),

                    State = i.State.Where(d => d.CostEffectiveness != null && d.CostEffectiveness > 0)
                            .Select(d => new BarChartItem<double?>
                            {
                                Value = d.CostEffectiveness,
                                Median = null,
                                Name = d.State.Name
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(d => d.CostEffectiveness != null && d.CostEffectiveness > 0)
                            .Select(d => new BarChartItem<double?>
                            {
                                Value = d.CostEffectiveness,
                                Median = null,
                                Name = "USA"
                            }).FirstOrDefault()
                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentageItem> Percentage(SizeUpContext context, long industryId, long placeId, double value)
        {
            var data = IndustryData.Get(context, industryId)
                .Where(i=>i.Place.Id == placeId)
                .Select(i => new
                {
                    County = i.County.Where(v => v.CostEffectiveness != null && v.CostEffectiveness > 0)
                            .Select(d => new
                            {
                                County = d.County,
                                Value = d.CostEffectiveness
                            }).FirstOrDefault(),

                    Metro = i.Metro.Where(v => v.CostEffectiveness != null && v.CostEffectiveness > 0)
                            .Select(d => new
                            {
                                Metro = d.Metro,
                                Value = d.CostEffectiveness
                            }).FirstOrDefault(),

                    State = i.State.Where(v => v.CostEffectiveness != null && v.CostEffectiveness > 0)
                            .Select(d => new
                            {
                                State = d.State,
                                Value = d.CostEffectiveness
                            }).FirstOrDefault(),

                    Nation = i.Nation.Where(v => v.CostEffectiveness != null && v.CostEffectiveness > 0)
                            .Select(d => new
                            {
                                Value = d.CostEffectiveness
                            }).FirstOrDefault()
                })
                .Select(i => new PlaceValues<PercentageItem>
                {
                    County = new PercentageItem
                    {
                        Percentage = i.County.Value != null ? (int?)(((value - i.County.Value) / i.County.Value) * 100) : null,
                        Name = i.County.County.Name + ", " + i.County.County.State.Abbreviation
                    },
                    Metro = new PercentageItem
                    {
                        Percentage = i.Metro.Value != null ? (int?)(((value - i.Metro.Value) / i.Metro.Value) * 100) : null,
                        Name = i.Metro.Metro.Name
                    },
                    State = new PercentageItem
                    {
                        Percentage = i.State.Value != null ? (int?)(((value - i.State.Value) / i.State.Value) * 100) : null,
                        Name = i.State.State.Name
                    },
                    Nation = new PercentageItem
                    {
                        Percentage = i.Nation.Value != null ? (int?)(((value - i.Nation.Value) / i.Nation.Value) * 100) : null,
                        Name = "USA"
                    }
                }).FirstOrDefault();

            return data;
        }
    }
}
