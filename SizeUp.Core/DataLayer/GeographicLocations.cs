using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Data.Spatial;
using SizeUp.Core.Geo;
using System.Data.Objects.SqlClient;


namespace SizeUp.Core.DataLayer
{
    public class GeographicLocation
    {
        public static IQueryable<Data.GeographicLocation> Get(SizeUpContext context)
        {
            return context.GeographicLocations;
        }

        public static IQueryable<Data.GeographicLocation> Get(SizeUpContext context, Granularity granularity)
        {
            var gran = Enum.GetName(typeof(Core.DataLayer.Granularity), granularity);
            return Get(context).Where(i => i.Granularity.Name == gran);
        }

        public static IQueryable<Data.GeographicLocation> In(SizeUpContext context, Granularity granularity, long placeId, Granularity boundingGranularity)
        {
            var geos = Get(context, granularity);
            if (boundingGranularity == Core.DataLayer.Granularity.City)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.City.Places.Any(p => p.Id == placeId)));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.County)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.County.Places.Any(p => p.Id == placeId)));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Metro)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.Metro.Counties.Any(c => c.Places.Any(p => p.Id == placeId))));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.State)
            {
                geos = geos.Where(i => i.GeographicLocations.Any(g => g.State.Counties.Any(c => c.Places.Any(p => p.Id == placeId))));
            }
            else if (boundingGranularity == Core.DataLayer.Granularity.Nation)
            {
                //NOOP
            }
            return geos;
        }


        public static IQueryable<Data.GeographicLocationRank> Ranks(SizeUpContext context)
        {
            return context.GeographicLocationRanks
                .Where(i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter);
        }

        

        public static IQueryable<Models.GeographicLocationRank> BestIndustries(SizeUpContext context, long geographicLocationId, string attribute)
        {
            var industries = Core.DataLayer.Industry.Get(context)
                .Select(new Projections.Industry.Default().Expression);

            var industryData = Core.DataLayer.IndustryData.Get(context)
                .Where(i=>i.GeographicLocationId == geographicLocationId);

            var ranks = Ranks(context)
                  .Where(i => i.GeographicLocationId == geographicLocationId);

            var data = industries.Join(industryData, i => i.Id, o => o.IndustryId, (i, o) => new { Industry = i, IndustryData = o })
                .Join(ranks, i => i.Industry.Id, o => o.IndustryId, (i, o) => new { i.Industry, i.IndustryData, Ranks = o });


            IQueryable<BestIndustryWrapper> output = new List<BestIndustryWrapper>().AsQueryable();

            if (attribute == IndustryAttribute.TotalRevenue)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Ranks.TotalRevenue, Value = i.IndustryData.TotalRevenue });
            }
            else if (attribute == IndustryAttribute.AverageRevenue)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Ranks.AverageRevenue, Value = i.IndustryData.AverageRevenue });
            }
            else if (attribute == IndustryAttribute.RevenuePerCapita)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Ranks.RevenuePerCapita, Value = i.IndustryData.RevenuePerCapita });
            }
            else if (attribute == IndustryAttribute.TotalEmployees)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Ranks.TotalEmployees, Value = i.IndustryData.TotalEmployees });
            }
            else if (attribute == IndustryAttribute.AverageEmployees)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Ranks.AverageEmployees, Value = i.IndustryData.AverageEmployees });
            }
            
            output = output
                .Where(i => i.Value != null)
                .Where(i => i.Rank != null)
                .OrderBy(i => i.Rank)
                .ThenByDescending(i => i.Value);




            return output
                .Select(i => new KeyValue<Models.Industry, long> { Key = i.Industry, Value = i.Rank.Value })
                .AsQueryable()
                .Select(new Projections.GeographicLocationRank.Default().Expression);
        }

        private class BestIndustryWrapper
        {
            public long? Rank { get; set; }
            public Models.Industry Industry { get; set; }
            public double? Value { get; set; }
        }

    }
}
