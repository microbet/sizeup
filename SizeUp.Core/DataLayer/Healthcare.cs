using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer
{
    public class Healthcare : Base.Base
    {
        public static HealthcareChart Chart(SizeUpContext context, long industryId, long placeId, long? employees, Granularity granularity)
        {
            HealthcareChart output = null;


            var data = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Cities.Any(c => c.CityCountyMappings.Any(m => m.Id == placeId)))
                .Select(i => new HealthcareChart
                {
                    Name = i.State.Name,
                    Industry = i.HealthcareByIndustry,
                    IndustryRank = i.HealthcareByIndustryRank,
                    State = i.HealthcareByState,
                    StateRank = i.HealthcareByStateRank,

                    FirmSize = employees == null ? null : employees <= 9 ? i.Healthcare0To9Employees : employees <= 24 ? i.Healthcare10To24Employees : employees <= 99 ? i.Healthcare25To99Employees : employees <= 999 ? i.Healthcare100To999Employees : i.Healthcare1000orMoreEmployees,
                    FirmSizeRank = employees == null ? null : employees <= 9 ? i.Healthcare0To9EmployeesRank : employees <= 24 ? i.Healthcare10To24EmployeesRank : employees <= 99 ? i.Healthcare25To99EmployeesRank : employees <= 999 ? i.Healthcare100To999EmployeesRank : i.Healthcare1000orMoreEmployeesRank

                });

            if (granularity == Granularity.State)
            {
                output = data.FirstOrDefault();
            }
            return output;
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long placeId, long value, Granularity granularity)
        {
            PercentageItem output = null;
            double healthcare = (double)value;
            var data = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Cities.Any(c => c.CityCountyMappings.Any(m => m.Id == placeId)))
                .Where(i=> i.HealthcareByState != null && i.HealthcareByState > 0)
                .Select(i => new PercentageItem
                {
                    Percentage = i.HealthcareByState > 0 ? (int?)(((healthcare - i.HealthcareByState) / i.HealthcareByState) * 100) : null,
                    Name = i.State.Name
                });

            if (granularity == Granularity.State)
            {
                output = data.FirstOrDefault();
            }
            return output;
        }
    }
}
