using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class WorkersCompChartItem : Base.ChartItem
    {
        public int? Rank {get;set;}
        public double? Average { get; set; }
    }
}
