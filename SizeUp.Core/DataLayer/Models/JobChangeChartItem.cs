using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class JobChangeChartItem : Base.ChartItem
    {
        public long? NetJobChange { get; set; }
        public long? JobGains { get; set; }
        public long? JobLosses { get; set; }
    }
}
