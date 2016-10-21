using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer.Projections
{
    public static class Healthcare
    {
        public class Chart : Projection<Data.IndustryData, Models.HealthcareChart>
        {
            public long? Employees { get; set; }

            public Chart()
            {
            }

            public Chart(long? employees)
            {
                Employees = employees;
            }
            public override Expression<Func<Data.IndustryData, Models.HealthcareChart>> Expression
            {
                get
                {
                    return i => new Models.HealthcareChart()
                    {
                        Name = i.GeographicLocation.LongName,
                        Industry = i.HealthcareByIndustry,
                        IndustryRank = i.HealthcareByIndustryRank,
                        State = i.HealthcareByState,
                        StateRank = i.HealthcareByStateRank,

                        FirmSize =     Employees == null ? null : Employees <= 9 ? i.Healthcare0To9Employees : Employees <= 24 ? i.Healthcare10To24Employees : Employees <= 99 ? i.Healthcare25To99Employees : Employees <= 999 ? i.Healthcare100To999Employees : i.Healthcare1000orMoreEmployees,
                        FirmSizeRank = Employees == null ? null : Employees <= 9 ? i.Healthcare0To9EmployeesRank : Employees <= 24 ? i.Healthcare10To24EmployeesRank : Employees <= 99 ? i.Healthcare25To99EmployeesRank : Employees <= 999 ? i.Healthcare100To999EmployeesRank : i.Healthcare1000orMoreEmployeesRank
                    };
                }
            }
        }
    }
}
