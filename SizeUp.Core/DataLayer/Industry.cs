using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
namespace SizeUp.Core.DataLayer
{
    public class Industry
    {
 
        public static IQueryable<Data.Industry> Get(SizeUpContext context)
        {
            return context.Industries.Where(i => i.IsActive && !i.IsDisabled);
        }


        public static Models.Industry Get(SizeUpContext context, long? id)
        {
            var p = new Projections.Industry.Default();
            return Get(context)
                .Where(i => i.Id == id)
                .Select(p.Expression)
                .FirstOrDefault();
        }


        public static Models.Industry Get(SizeUpContext context, string SEOKey)
        {
            var p = new Projections.Industry.Default();
            return Get(context)
                .Where(i => i.SEOKey == SEOKey)
                .Select(p.Expression)
                .FirstOrDefault();
        }


        public static Models.Industry GetLegacy(SizeUpContext context, string SEOKey)
        {
            var p = new Projections.Industry.Default();
            return Get(context)
                .Where(i => i.LegacyIndustrySEOKeys.Any(l => l.SEOKey == SEOKey))
                .Select(p.Expression)
                .FirstOrDefault();
        }


        public static IQueryable<Models.Industry> Get(SizeUpContext context, List<long> ids)
        {
            var p = new Projections.Industry.Default();
            return Get(context)
                .Where(i => ids.Contains(i.Id))
                .Select(p.Expression);
        }

        public static IQueryable<Models.Industry> ListInPlace(SizeUpContext context, long placeId)
        {
            var p = new Projections.Industry.Default();

            return context.Industries
                .SelectMany(i => i.IndustryDatas, (i, o) => new { Industry = i, IndustryData = o })
                .Where(i => i.Industry.IsActive && !i.Industry.IsDisabled)
                .Where(i => i.IndustryData.Year == CommonFilters.TimeSlice.Industry.Year && i.IndustryData.Quarter == CommonFilters.TimeSlice.Industry.Quarter)
                .Where(i => i.IndustryData.GeographicLocation.City.Places.Any(pl => pl.Id == placeId))
                .Where(i => i.IndustryData.BusinessCount > 0)
                .Select(i => i.Industry)
                .Select(p.Expression);
        }


        public static IQueryable<Models.Industry> Search(SizeUpContext context, string term)
        {
            var qs = term.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            var industries = Get(context);

            return industries
                    .Select(i => new
                    {
                        SortOrder = 1,
                        Industry = new Models.Industry
                        {
                            Id = i.Id,
                            Name = i.Name,
                            SEOKey = i.SEOKey,
                            SICCode = i.SicCode,
                            ParentName = i.Parent.Name,
                            NAICS6 = new Models.NAICS
                            {
                                Id = i.NAICS.Id,
                                NAICSCode = i.NAICS.NAICSCode,
                                Name = i.NAICS.Name
                            },
                            NAICS4 = new Models.NAICS
                            {
                                Id = i.NAICS.Parent.Id,
                                NAICSCode = i.NAICS.Parent.NAICSCode,
                                Name = i.NAICS.Parent.Name
                            }
                        }
                    })
                    .Union(industries
                        .SelectMany(i => i.IndustryKeywords)
                        .Select(i => new
                        {
                            i.SortOrder,
                            Industry = new Models.Industry
                            {
                                Id = i.Id,
                                Name = i.Name,
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
                            }
                        }))
                    .Where(i => qs.All(d => i.Industry.Name.Contains(d)))
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.Industry);
        }
    }
}
