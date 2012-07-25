using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;


namespace SizeUp.Web.Areas.Api.Models.WorkersComp
{
    public class ChartItem : Charts.BarChartItem
    {
        public double Average { get; set; }
        public int Rank { get; set; }   
    }
}