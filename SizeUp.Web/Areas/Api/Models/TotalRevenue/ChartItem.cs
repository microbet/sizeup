using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;
namespace SizeUp.Web.Areas.Api.Models.TotalRevenue
{
    public class ChartItem : Models.Charts.BarChartItem
    {
        public long? Value { get; set; }
        public long? Median { get; set; }
    }
}