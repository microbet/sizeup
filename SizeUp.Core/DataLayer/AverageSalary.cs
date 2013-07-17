using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class AverageSalary
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.GetMinimumBusinessCount(context)
                .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocationId == place.FirstOrDefault().CityId);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocationId == place.FirstOrDefault().CountyId);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocationId == place.FirstOrDefault().County.MetroId);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocationId == place.FirstOrDefault().County.StateId);
            }
            else if (granularity == Granularity.Nation)
            {
                //NOOP data = data;
            }
            return data
                .Select(new Projections.AverageSalary.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentageItem Percentage(SizeUpContext context, long industryId, long placeId, long value, Granularity granularity)
        {
            PercentageItem output = null;

            var data = Core.DataLayer.IndustryData.Get(context)
                        .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);


            if (granularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.FirstOrDefault().CityId);
            }
            else if (granularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.FirstOrDefault().CountyId);
            }
            else if (granularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.FirstOrDefault().County.MetroId);
            }
            else if (granularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.Id == place.FirstOrDefault().County.StateId);
            }
            else if (granularity == Granularity.Nation)
            {
                //NOOP data = data;
            }

            return data.Select(i => new PercentageItem
            {
                Name = i.GeographicLocation.LongName,
                Percentage = (long)(((value - i.AverageAnnualSalary) / (decimal)i.AverageAnnualSalary)) * 100
            })
                .FirstOrDefault(); 
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.GetMinimumBusinessCount(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);


            if (boundingGranularity == Granularity.City)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.FirstOrDefault().CityId));
            }
            else if (boundingGranularity == Granularity.County)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.FirstOrDefault().CountyId));
            }
            else if (boundingGranularity == Granularity.Metro)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.FirstOrDefault().County.MetroId));
            }
            else if (boundingGranularity == Granularity.State)
            {
                data = data.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == place.FirstOrDefault().County.StateId));
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                //NOOP data = data;
            }



            var output = data.Select(i => i.AverageAnnualSalary)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
