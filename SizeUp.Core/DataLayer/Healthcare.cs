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
        public static HealthcareChart Chart(SizeUpContext context, long industryId, long placeId, long? employees)
        {

            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i => i.State.Select(s=>s).FirstOrDefault())
                .Select(i => new HealthcareChart
                {
                    Industry = i.HealthcareByIndustry,
                    IndustryRank = i.HealthcareByIndustryRank,
                    State = i.HealthcareByState,
                    StateRank = i.HealthcareByStateRank,

                    FirmSize = employees == null ? null : employees <= 9 ? i.Healthcare0To9Employees : employees <= 24 ? i.Healthcare10To24Employees : employees <= 99 ? i.Healthcare25To99Employees : employees <= 999 ? i.Healthcare100To999Employees : i.Healthcare1000orMoreEmployees,
                    FirmSizeRank = employees == null ? null : employees <= 9 ? i.Healthcare0To9EmployeesRank : employees <= 24 ? i.Healthcare10To24EmployeesRank : employees <= 99 ? i.Healthcare25To99EmployeesRank : employees <= 999 ? i.Healthcare100To999EmployeesRank : i.Healthcare1000orMoreEmployeesRank

                }).FirstOrDefault();
            return data;
        }

        public static PlaceValues<PercentageItem> Percentage(SizeUpContext context, long industryId, long placeId, long value)
        {
            double healthcare = (double)value;
            var data = IndustryData.Get(context, industryId)
                .Where(i => i.Place.Id == placeId)
                .Select(i=> new 
                {
                    State = i.State.Where(v => v.HealthcareByState != null && v.HealthcareByState > 0)
                            .Select(d => new
                            {
                                State = d.State,
                                Value = d.HealthcareByState
                            }).FirstOrDefault()
                })
                .Select(i => new PlaceValues<PercentageItem>
                {
                    State = new PercentageItem
                    {
                        Percentage = i.State.Value != null ? (int?)(((healthcare - i.State.Value) / i.State.Value) * 100) : null,
                        Name = i.State.State.Name
                    }
                }).FirstOrDefault();

            return data;
        }
    }
}
