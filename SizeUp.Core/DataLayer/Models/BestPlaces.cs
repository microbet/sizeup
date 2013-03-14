using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Models
{
    public class BestPlaces
    {
        public Models.Place Place { get; set; }
        public Core.Geo.LatLng Centroid { get; set; }
        public Core.Geo.BoundingBox BoundingBox { get; set; }

        public long? AverageRevenue { get; set; }
        public long? TotalRevenue { get; set; }
        public long? TotalEmployees { get; set; }
        public long? RevenuePerCapita { get; set; }
        public long? HouseholdIncome { get; set; }
        public long? AverageEmployees { get; set; }
        public long? AirportsNearby { get; set; }
        public long? UniversitiesNearby { get; set; }
        public double? CommuteTime { get; set; }

        public long? Population { get; set; }

        public double? EmployeesPerCapita { get; set; }
        public double? BachelorsDegreeOrHigher { get; set; }
        public double? HighSchoolOrHigher { get; set; }
        public double? WhiteCollarWorkers { get; set; }
        public double? MedianAge { get; set; }
        public double? HouseholdExpenditures { get; set; }
        public double? YoungEducated { get; set; }
        public double? BlueCollarWorkers { get; set; }
    }

    public class BestPlacesOutput
    {
        public Models.Place Place { get; set; }
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
