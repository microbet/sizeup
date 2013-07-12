using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class BarChartItem<T> : Base.ChartItem
    {
        public T Value { get; set; }
        public T Median { get; set; }
    }
}
