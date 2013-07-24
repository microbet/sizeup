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
    public static class Turnover
    {
        public class Chart : Projection<Data.IndustryData, Models.TurnoverChartItem>
        {
            public override Expression<Func<Data.IndustryData, Models.TurnoverChartItem>> Expression
            {
                get
                {
                    return i => new Models.TurnoverChartItem()
                    {
                        Name = i.GeographicLocation.LongName,
                        Turnover = i.TurnoverRate *100,
                        Hires = i.Hires,
                        Separations = i.Separations
                    };
                }
            }
        }
    }
}
