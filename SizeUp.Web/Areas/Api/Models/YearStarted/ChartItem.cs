using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;

namespace SizeUp.Web.Areas.Api.Models.YearStarted
{
    public class ChartItem : Charts.LineChartItem
    {
        public class ChartItemValue : Charts.LineChartItem.LineChartItemValue<int, int>
        {

        }
        public IEnumerable<ChartItemValue> Values { get; set; }
    }

}