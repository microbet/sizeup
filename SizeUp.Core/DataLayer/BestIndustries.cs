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
using System.Data.Objects;


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
            var data = context.Industries
                .SelectMany(i => i.IndustryDatas, (i, o) => new { Industry = i, IndustryData = o })
                .SelectMany(i => i.Industry.GeographicLocationRanks, (i, o) => new { i.Industry, i.IndustryData, Rank = o })
                .Where(i => i.Industry.IsActive && !i.Industry.IsDisabled)
                .Where(i => i.IndustryData.Year == CommonFilters.TimeSlice.Industry.Year && i.IndustryData.Quarter == CommonFilters.TimeSlice.Industry.Quarter)
                .Where(i => i.Rank.Year == CommonFilters.TimeSlice.Industry.Year && i.Rank.Quarter == CommonFilters.TimeSlice.Industry.Quarter)
                .Where(i => i.Rank.GeographicLocationId == geographicLocationId)
                .Where(i => i.IndustryData.GeographicLocationId == geographicLocationId);      

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
                .Select(i => new Core.DataLayer.Models.KeyValue<Core.DataLayer.Models.Industry, long> {
                    Key = new Models.Industry
                    {
                        Id = i.Industry.Id,
                        Name = i.Industry.Name,
                        SEOKey = i.Industry.SEOKey,
                        SICCode = i.Industry.SicCode,
                        ParentName = i.Industry.Parent.Name,
                        NAICS6 = new Models.NAICS
                        {
                            Id = i.Industry.NAICS.Id,
                            NAICSCode = i.Industry.NAICS.NAICSCode,
                            Name = i.Industry.NAICS.Name
                        },
                        NAICS4 = new Models.NAICS
                        {
                            Id = i.Industry.NAICS.Parent.Id,
                            NAICSCode = i.Industry.NAICS.Parent.NAICSCode,
                            Name = i.Industry.NAICS.Parent.Name
                        }
                    },
                    Value = i.Rank.Value
                })
                .AsQueryable()
                .Select(new Core.DataLayer.Projections.GeographicLocationRank.Default().Expression);
        }

        private class BestIndustryWrapper
        {
            public long? Rank { get; set; }
            public Data.Industry Industry { get; set; }
            public double? Value { get; set; }
        }
    }
}
