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
    public static class WorkersComp
    {
        public class Chart : Projection<Data.IndustryData, Models.WorkersCompChartItem>
        {
            public override Expression<Func<Data.IndustryData, Models.WorkersCompChartItem>> Expression
            {
                get
                {
                    return i => new Models.WorkersCompChartItem()
                    {
                        Name = i.GeographicLocation.LongName,
                        Average = i.WorkersComp,
                        Rank = i.WorkersCompRank
                    };
                }
            }
        }
    }
}
