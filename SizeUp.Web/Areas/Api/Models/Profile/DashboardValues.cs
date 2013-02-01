using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SizeUp.Web.Areas.Api.Models.Profile
{
    public class DashboardValues
    {
        public int? Revenue { get; set; }
        public int? YearStarted { get; set; }
        public int? Salary { get; set; }
        public int? Employees { get; set; }
        public int? HealthcareCost { get; set; }
        public decimal? WorkersComp { get; set; }
        public string BusinessSize { get; set; }
        public string BusinessType { get; set; }
    }
}