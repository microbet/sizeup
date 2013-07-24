using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer.Projections
{
    public static class CostEffectiveness
    {
        public class Chart : Projection<Data.IndustryData, Models.BarChartItem<double?>>
        {
            public override Expression<Func<Data.IndustryData, Models.BarChartItem<double?>>> Expression
            {
                get
                {
                    return i => new Models.BarChartItem<double?>()
                    {
                        Value = i.CostEffectiveness,
                        Median = null,
                        Name = i.GeographicLocation.LongName
                    };
                }
            }
        }
    }
}
