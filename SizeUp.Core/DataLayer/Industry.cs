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
            return Get(context)
                .Where(i => i.IndustryDatas.Any(d => d.GeographicLocation.City.Places.Any(pl => pl.Id == placeId)))
                .Select(p.Expression);
        }


        public static IQueryable<Models.Industry> Search(SizeUpContext context, string term)
        {
            var qs = term.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            var industries = Get(context);
            return industries
                    .Select(i => new
                    {
                        Id = i.Id,
                        i.Name,
                        i.SEOKey,
                        SortOrder = 1
                    })
                    .Union(industries
                        .SelectMany(i => i.IndustryKeywords)
                        .Select(i => new
                        {
                            Id = i.IndustryId,
                            i.Name,
                            i.Industry.SEOKey,
                            i.SortOrder
                        }))
                    .Where(i => qs.All(d => i.Name.Contains(d)))
                    .OrderBy(i=>i.SortOrder)
                    .Select(i => new Models.Industry
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey
                    });
        }
    }
}
