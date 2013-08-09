using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;

namespace SizeUp.Core.DataLayer
{
    public class WorkersComp 
    {
        public static WorkersCompChartItem Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
               .Where(i => i.IndustryId == industryId)
               .Where(i => i.GeographicLocationId == geographicLocationId);

            return data
                .Select(new Projections.WorkersComp.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long geographicLocationId, double value)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                        .Where(i => i.IndustryId == industryId)
                        .Where(i => i.GeographicLocationId == geographicLocationId);

            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)((((value - i.WorkersComp) / (double)i.WorkersComp)) * 100)
            })
                .FirstOrDefault(); 
        }
    }
}
