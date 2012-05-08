﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Charts
{
    public class BarChart
    {
        public ChartItem National { get; set; }
        public ChartItem State { get; set; }
        public ChartItem Metro { get; set; }
        public ChartItem County { get; set; }
    }
}