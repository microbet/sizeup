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
        public static WorkersCompChartItem Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context, granularity)
                .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.List(context)
               .Where(i => i.Id == placeId)
               .FirstOrDefault();


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.City.Id);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.County.Id);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Metro.Id);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.State.Id);
            }
            else if (granularity == Granularity.Nation)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Nation.Id);
            }
            return data
                .Select(new Projections.WorkersComp.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long placeId, double value, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                       .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.List(context)
               .Where(i => i.Id == placeId)
               .FirstOrDefault();


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.City.Id);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.County.Id);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Metro.Id);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.State.Id);
            }
            else if (granularity == Granularity.Nation)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.Nation.Id);
            }

            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)((((value - i.WorkersComp) / (double)i.WorkersComp)) * 100)
            })
                .FirstOrDefault(); 
        }
    }
}
