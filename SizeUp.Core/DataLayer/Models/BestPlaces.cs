using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class BestPlaces
    {
        public Models.City City { get; set; }
        public Models.County County { get; set; }
        public Models.Metro Metro { get; set; }
        public Models.State State { get; set; }
        public Models.Industry Industry { get; set; }
        public Core.Geo.LatLng Centroid { get; set; }
        public Core.Geo.BoundingBox BoundingBox { get; set; }
        public Band<double> TotalRevenue { get; set; }
        public Band<double> TotalEmployees { get; set; }
        public Band<double> AverageRevenue { get; set; }
        public Band<double> AverageEmployees { get; set; }
        public Band<double> RevenuePerCapita { get; set; }
        public Band<double> EmployeesPerCapita { get; set; }

    }
}
