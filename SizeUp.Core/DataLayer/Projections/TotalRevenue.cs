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
    public static class TotalRevenue
    {
        public class Chart : Projection<Data.IndustryData, Models.BarChartItem<long?>>
        {
            public override Expression<Func<Data.IndustryData, Models.BarChartItem<long?>>> Expression
            {
                get
                {
                    return i => new Models.BarChartItem<long?>()
                    {
                        Value = i.TotalRevenue,
                        Median = null,
                        Name = i.GeographicLocation.LongName
                    };
                }
            }
        }
    }
}
