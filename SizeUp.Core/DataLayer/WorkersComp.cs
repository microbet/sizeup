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
    public class WorkersComp : Base.Base
    {
        public static WorkersCompChartItem Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            WorkersCompChartItem output = null;


            var data = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Cities.Any(c => c.CityCountyMappings.Any(m => m.Id == placeId)))
                .Select(i => new WorkersCompChartItem
                {
                    Name = i.State.Name,
                    Average = i.WorkersComp,
                    Rank = i.WorkersCompRank
                });

            if (granularity == Granularity.State)
            {
                output = data.FirstOrDefault();
            }
            return output;
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long placeId, double value, Granularity granularity)
        {
            PercentageItem output = null;
            var data = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Cities.Any(c => c.CityCountyMappings.Any(m => m.Id == placeId)))
                .Where(i => i.WorkersComp != null && i.WorkersComp > 0)
                .Select(i => new PercentageItem
                {
                    Percentage = i.WorkersComp > 0 ? (int?)(((value - i.WorkersComp) / i.WorkersComp) * 100) : null,
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
