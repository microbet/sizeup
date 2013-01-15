using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SizeUp.Web.Areas.Api.Models.Shared;

namespace SizeUp.Web.Areas.Api.Models.TopPlaces
{
    public class TopPlace
    {
        public Models.City.City City { get; set; }
        public Models.County.County County { get; set; }
        public Models.Metro.Metro Metro { get; set; }
        public Models.State.State State { get; set; }

        public Band<long?> TotalRevenue { get; set; }
        public Band<long?> TotalEmployees { get; set; }
        public Band<decimal?> EmployeesPerCapita { get; set; }
        public Band<long?> RevenuePerCapita { get; set; }
        public Band<long?> AverageRevenue { get; set; }
        public Band<long?> AverageEmployees { get; set; }
    }
}