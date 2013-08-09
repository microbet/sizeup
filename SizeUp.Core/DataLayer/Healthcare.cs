using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;


namespace SizeUp.Core.DataLayer
{
    public class Healthcare
    {
        public static HealthcareChart Chart(SizeUpContext context, long industryId, long geographicLocationId, long? employees)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
                .Select(new Projections.Healthcare.Chart(employees).Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long geographicLocationId, long value )
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                         .Where(i => i.IndustryId == industryId)
                         .Where(i => i.GeographicLocationId == geographicLocationId);


            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)((((value - i.HealthcareByState) / (decimal)i.HealthcareByState)) * 100)
            })
                .FirstOrDefault(); 
        }
    }
}
