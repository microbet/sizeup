using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Charts
{
    public class BarChart
    {
        public BarChartItem Nation { get; set; }
        public BarChartItem State { get; set; }
        public BarChartItem Metro { get; set; }
        public BarChartItem County { get; set; }
        public BarChartItem City { get; set; }
    }
}