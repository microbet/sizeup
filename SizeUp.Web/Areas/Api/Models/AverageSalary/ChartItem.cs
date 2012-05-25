using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models;
namespace SizeUp.Web.Areas.Api.Models.AverageSalary
{
    public class ChartItem : Models.Charts.ChartItem
    {
        public long? Value { get; set; }
    }
}