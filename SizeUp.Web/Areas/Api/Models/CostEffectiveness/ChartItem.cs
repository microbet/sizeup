using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;
namespace SizeUp.Web.Areas.Api.Models.CostEffectiveness
{
    public class ChartItem : Models.Charts.BarChartItem
    {
        public double? Value { get; set; }
    }
}