using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class LineChartItem<T, V> : ChartItem
    {
        public T Key { get; set; }
        public V Value { get; set; }
    }
}
