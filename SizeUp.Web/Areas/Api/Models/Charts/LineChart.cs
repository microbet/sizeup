using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Charts
{
    public class LineChart
    {
        public LineChartItem City { get; set; }
        public LineChartItem County { get; set; }
        public LineChartItem Metro { get; set; }
        public LineChartItem State { get; set; }
        public LineChartItem Nation { get; set; }
    }
}