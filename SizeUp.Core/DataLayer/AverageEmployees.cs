﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Data;
using SizeUp.Core.Extensions;


namespace SizeUp.Core.DataLayer
{
    public class AverageEmployees
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long placeId, Granularity granularity)
        {
            var data = Core.DataLayer.IndustryData.GetMinimumBusinessCount(context, granularity)
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
                .Select(new Projections.AverageEmployees.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long placeId, long value, Granularity granularity)
        {
            var data = Core.DataLayer.BusinessData.Get(context)
                        .Where(i => i.Industry.Id == industryId)
                        .Where(i => i.Employees != null && i.Employees > 0);

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



            var output = data.Select(i=> new 
            {
                Name = i.GeographicLocation.LongName,
                Filtered = data.Count(c=>c.Employees <= value),
                Total = data.Count()
            }).FirstOrDefault();


            return output.Total > CommonFilters.MinimumBusinessCount ? new PercentileItem
            {
                Name = output.Name,
                Percentile = (((decimal)output.Filtered / ((decimal)output.Total + 1) * 100))
            } : null;
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var data = Core.DataLayer.IndustryData.Get(context, granularity, placeId, boundingGranularity)
                .Where(i => i.IndustryId == industryId);

            var output = data.Select(i => i.AverageEmployees)
                .Where(i => i != null && i > 0)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
