using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.TopPlaces
{
    public class TopPlace
    {
        public Models.City.City City { get; set; }
        public Models.County.County County { get; set; }
        public Models.Metro.Metro Metro { get; set; }
        public Models.State.State State { get; set; }
        public long? TotalRevenue { get; set; }
        public long? TotalEmployees { get; set; }
        public double? EmployeesPerCapita { get; set; }
        public long? RevenuePerCapita { get; set; }
        public long? AverageRevenue { get; set; }
        public long? AverageEmployees { get; set; }

    }
}