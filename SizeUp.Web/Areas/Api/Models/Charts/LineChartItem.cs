using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Charts
{
    public class LineChartItem
    {
        public class LineChartItemValue<T,V>
        {
            public T Key { get; set; }
            public V Value { get; set; }
        }
        public string Name { get; set; }
    }
}