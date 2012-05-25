using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Turnover
{
    public class TurnoverDynamics
    {
        public double? Turnover { get; set; }
        public long? Hires { get; set; }
        public long? Separations { get; set; }
    }
}