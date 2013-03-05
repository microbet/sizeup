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
    public class JobChange : Base.Base
    {
        public static JobChangeChartItem Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            JobChangeChartItem output = null;

            var countyData = IndustryData.County(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.County.CityCountyMappings.Any(m => m.Id == placeId));

            var metroData = IndustryData.Metro(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.Metro.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));

            var stateData = IndustryData.State(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.State.Counties.Any(m => m.CityCountyMappings.Any(mp => mp.Id == placeId)));

            var nationData = IndustryData.Nation(context)
                .Where(i => i.IndustryId == industryId);


            var county = countyData.Select(i => new JobChangeChartItem
            {
                JobGains = i.JobGains,
                JobLosses = i.JobLosses,
                NetJobChange = i.NetJobChange,
                Name = i.County.Name + ", " + i.County.State.Abbreviation
            });

            var metro = metroData.Select(i => new JobChangeChartItem
            {
                JobGains = i.JobGains,
                JobLosses = i.JobLosses,
                NetJobChange = i.NetJobChange,
                Name = i.Metro.Name
            });

            var state = stateData.Select(i => new JobChangeChartItem
            {
                JobGains = i.JobGains,
                JobLosses = i.JobLosses,
                NetJobChange = i.NetJobChange,
                Name = i.State.Name
            });

            var nation = nationData.Select(i => new JobChangeChartItem
            {
                JobGains = i.JobGains,
                JobLosses = i.JobLosses,
                NetJobChange = i.NetJobChange,
                Name = "USA"
            });

            if (granularity == Granularity.County)
            {
                output = county.FirstOrDefault();
            }
            else if (granularity == Granularity.Metro)
            {
                output = metro.FirstOrDefault();
            }
            else if (granularity == Granularity.State)
            {
                output = state.FirstOrDefault();
            }
            else if (granularity == Granularity.Nation)
            {
                output = nation.FirstOrDefault();
            }
            return output;
        }
    }
}
