using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;
namespace SizeUp.Web.Areas.Api.Models.Turnover
{
    public class ChartItem : Charts.BarChartItem
    {
        public long? Hires { get; set; }
        public long? Separations { get; set; }
        public double? Turnover { get; set; }
    }
}