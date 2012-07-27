using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Charts
{
    public class LineChart<T, V>
    {
        public List<LineChartItem<T, V>> City { get; set; }
        public List<LineChartItem<T, V>> County { get; set; }
        public List<LineChartItem<T, V>> Metro { get; set; }
        public List<LineChartItem<T, V>> State { get; set; }
        public List<LineChartItem<T, V>> Nation { get; set; }
    }
}