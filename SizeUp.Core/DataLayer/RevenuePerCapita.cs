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
    public class RevenuePerCapita
    {
        public static BarChartItem<long?> Chart(SizeUpContext context, long industryId, long geographicLocationId)
        {
            var data = Core.DataLayer.IndustryData.Get(context)
                 .Where(i => i.IndustryId == industryId && i.BusinessCount > CommonFilters.MinimumBusinessCount)
                 .Where(i => i.RevenuePerCapita != null)
                 .Where(i => i.GeographicLocationId == geographicLocationId);
            
            return data
                .Select(new Projections.RevenuePerCapita.Chart().Expression)
                .FirstOrDefault();
        }

        public static PercentileItem Percentile(SizeUpContext context, long industryId, long geographicLocationId, long boundingGeographicLocationId)
        {
            PercentileItem output = null;
            var gran = Enum.GetName(typeof(Granularity), Granularity.City);

            var raw = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);

            var value = raw.Where(i => i.GeographicLocationId == geographicLocationId).Select(i => i.RevenuePerCapita);

            raw = raw.Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));
            output = raw.Select(i => new
            {
                i.GeographicLocation.LongName,
                Total = raw.Count(),
                Filtered = raw.Count(c => c.RevenuePerCapita >= value.FirstOrDefault())
            })
            .Select(i => new PercentileItem
            {
                Name = i.LongName,
                Percentile = (((decimal)i.Filtered / ((decimal)i.Total) * 100))
            }).FirstOrDefault();
            
            return output;
        }

        public static List<Band<long>> Bands(SizeUpContext context, long industryId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Granularity), granularity);

            var data = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.IndustryId == industryId)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            var output = data
                .Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0)
                .Select(i => i.Bands.Where(b => b.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Min.Value, Max = (double)b.Max.Value }).FirstOrDefault())
                .ToList()
                .NTileDescending(i => i.Min, bands)
                .Select(i => new Band<long>() { Min = (long)i.Min(v => v.Min), Max = (long)i.Max(v => v.Max) })
                .ToList();

            output.FormatDescending();
            return output;
        }
    }
}
