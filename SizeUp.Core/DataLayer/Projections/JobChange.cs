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
    public static class JobChange
    {
        public class Chart : Projection<Data.IndustryData, Models.JobChangeChartItem>
        {
            public override Expression<Func<Data.IndustryData, Models.JobChangeChartItem>> Expression
            {
                get
                {
                    return i => new Models.JobChangeChartItem()
                    {
                        Name = i.GeographicLocation.LongName,
                        JobGains = i.JobGains,
                        JobLosses = i.JobLosses,
                        NetJobChange =i .NetJobChange
                    };
                }
            }
        }
    }
}
