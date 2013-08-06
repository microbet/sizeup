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
    public class BestIndustries
    {
        public static IQueryable<Data.GeographicLocationRank> Ranks(SizeUpContext context)
        {
            return context.GeographicLocationRanks
                .Where(i => i.Year == CommonFilters.TimeSlice.Industry.Year && i.Quarter == CommonFilters.TimeSlice.Industry.Quarter);
        }



        public static IQueryable<Core.DataLayer.Models.GeographicLocationRank> Get(SizeUpContext context, long geographicLocationId, string attribute)
        {
            var industries = Core.DataLayer.Industry.Get(context)
                .Select(new Core.DataLayer.Projections.Industry.Default().Expression);

            var industryData = Core.DataLayer.IndustryData.Get(context)
                .Where(i => i.GeographicLocationId == geographicLocationId);

            var ranks = Ranks(context)
                  .Where(i => i.GeographicLocationId == geographicLocationId)
                  .Select(i => new { Rank = i, IndustryData = i.GeographicLocation.IndustryDatas.Where(d => d.Year == CommonFilters.TimeSlice.Industry.Year && d.Quarter == CommonFilters.TimeSlice.Industry.Quarter && d.IndustryId == i.IndustryId).FirstOrDefault() });

            var data = industries.Join(ranks, i => i.Id, o => o.Rank.IndustryId, (i, o) => new { Industry = i, Rank = o.Rank, IndustryData = o.IndustryData });


            IQueryable<BestIndustryWrapper> output = new List<BestIndustryWrapper>().AsQueryable();

            if (attribute == IndustryAttribute.TotalRevenue)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Rank.TotalRevenue, Value = i.IndustryData.TotalRevenue });
            }
            else if (attribute == IndustryAttribute.AverageRevenue)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Rank.AverageRevenue, Value = i.IndustryData.AverageRevenue });
            }
            else if (attribute == IndustryAttribute.RevenuePerCapita)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Rank.RevenuePerCapita, Value = i.IndustryData.RevenuePerCapita });
            }
            else if (attribute == IndustryAttribute.TotalEmployees)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Rank.TotalEmployees, Value = i.IndustryData.TotalEmployees });
            }
            else if (attribute == IndustryAttribute.AverageEmployees)
            {
                output = data.Select(i => new BestIndustryWrapper { Industry = i.Industry, Rank = i.Rank.AverageEmployees, Value = i.IndustryData.AverageEmployees });
            }

            output = output
                .Where(i => i.Value != null)
                .Where(i => i.Rank != null)
                .OrderBy(i => i.Rank)
                .ThenByDescending(i => i.Value);




            return output
                .Select(i => new Core.DataLayer.Models.KeyValue<Core.DataLayer.Models.Industry, long> { Key = i.Industry, Value = i.Rank.Value })
                .AsQueryable()
                .Select(new Core.DataLayer.Projections.GeographicLocationRank.Default().Expression);
        }

        private class BestIndustryWrapper
        {
            public long? Rank { get; set; }
            public Core.DataLayer.Models.Industry Industry { get; set; }
            public double? Value { get; set; }
        }
    }
}
