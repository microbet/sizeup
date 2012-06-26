using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Models.JobChange
{
    public class ChartItem : Charts.BarChartItem
    {
        public long? JobLosses { get; set; }
        public long? JobGains { get; set; }
        public long? NetJobChange { get; set; }
    }
}