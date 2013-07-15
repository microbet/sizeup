using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.Geo;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class AverageRevenue
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(new Filters.IndustryData.MinimumBusinessCount().Expression)
                .Where(i => i.IndustryId == industryId);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i=>i.Id == placeId);


            if(granularity == Granularity.City)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().CityId);
            }
            else if(granularity == Granularity.County)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().CountyId);
            }
            else if(granularity == Granularity.Metro)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().County.MetroId);
            }
            else if(granularity == Granularity.State)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().County.StateId);
            }
            else if(granularity == Granularity.Nation)
            {
                //NOOP data = data;
            }
            return data
                .Select(new Projections.AverageRevenue.Chart().Expression)
                .FirstOrDefault();
        }


        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, long value, Granularity granularity)
        {

            var data = Core.DataLayer.BusinessData.Get(context)
                        .GroupBy(i => new { i.GeographicLocation, i.Industry })
                        .Select(i => new
                        {
                            i.Key.GeographicLocation,
                            i.Key.Industry,
                            Total = i.Count(),
                            Filtered = i.Count(v => v.Revenue <= value)
                        })
                        .Where(i => i.Industry.Id == industryId)
                        .Where(i=>i.Total > 0)
                        .Where(i=>i.Total >= CommonFilters.MinimumBusinessCount);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i => i.Id == placeId);


            if(granularity == Granularity.City)
            {
                data = data.Where(i=>i.GeographicLocation.Id == place.FirstOrDefault().CityId);
            }
            else if(granularity == Granularity.County)
            {
                data = data.Where(i=>i.GeographicLocation.Id == place.FirstOrDefault().CountyId);
            }
            else if(granularity == Granularity.Metro)
            {
                data = data.Where(i=>i.GeographicLocation.Id == place.FirstOrDefault().County.MetroId);
            }
            else if(granularity == Granularity.State)
            {
                data = data.Where(i=>i.GeographicLocation.Id == place.FirstOrDefault().County.StateId);
            }
            else if(granularity == Granularity.Nation)
            {
                //NOOP data = data;
            }         
                        
            return data.Select(i=> new PercentileItem
                {
                    Name = i.GeographicLocation.LongName,
                    Percentile = (int?)(((decimal)i.Filtered / (decimal)i.Total + 1) * 100)
                })
                .FirstOrDefault(); 
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity = Granularity.Nation)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(new Filters.IndustryData.MinimumBusinessCount().Expression)
                .Where(i => i.IndustryId == industryId)
                .Where(i=>i.GeographicLocation.Granularity.Name == gran);

            var place = Core.DataLayer.Place.Get(context)
                .Where(i=>i.Id == placeId);


           


            if(boundingGranularity == Granularity.City)
            {
                data = data.Where(i=>i.GeographicLocation.ZipCode.Places.Any(z=>z.CityId == place.FirstOrDefault().CityId));
            }
            else if(boundingGranularity == Granularity.County)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().CountyId);
            }
            else if(boundingGranularity == Granularity.Metro)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().County.MetroId);
            }
            else if(boundingGranularity == Granularity.State)
            {
                data = data.Where(i=>i.GeographicLocationId == place.FirstOrDefault().County.StateId);
            }
            else if(boundingGranularity == Granularity.Nation)
            {
                //NOOP data = data;
            }



                

            if (boundingGranularity == Granularity.City)
            {
                var entities = Base.ZipCode.In(context, placeId, boundingGranularity);
                var data = IndustryData.ZipCode(context).Where(i => i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.ZipCodeId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            else if (boundingGranularity == Granularity.County)
            {
                var entities = Base.County.In(context, placeId, boundingGranularity);
                var data = IndustryData.County(context).Where(i => i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.CountyId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            else if (boundingGranularity == Granularity.State)
            {
                raw = raw.Where(i=>i.GeographicLocation.
                var entities = Base.State.In(context, placeId, boundingGranularity);
                var data = IndustryData.State(context).Where(i => i.IndustryId == industryId);
                averageRevenue = 
                    data.Join(entities, i => i.StateId, i => i.Id, (d, e) => d)
                    .Select(i=>i.AverageRevenue);
            }
            else if (boundingGranularity == Granularity.Nation)
            {
                raw = raw;
            }



            var output = averageRevenue
                .Where(i=>i!= null && i > 0)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
