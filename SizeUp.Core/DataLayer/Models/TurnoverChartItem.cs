using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class TurnoverChartItem : Base.ChartItem
    {
        public long? Hires { get; set; }
        public long? Separations { get; set; }
        public double? Turnover { get; set; }
    }
}
