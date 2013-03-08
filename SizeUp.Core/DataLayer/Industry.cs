using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
namespace SizeUp.Core.DataLayer
{
    public class Industry
    {
        public static Models.Industry Get(SizeUpContext context, long? id)
        {
            var data = Base.Industry.Get(context)
                .Where(i => i.Id == id)
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                }).FirstOrDefault();
            return data;
        }

        public static List<Models.Industry> List(SizeUpContext context, List<long> industryIds)
        {
            var data = Base.Industry.Get(context)
                .Where(i => industryIds.Contains(i.Id))
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                })
                .ToList();
            return data;
        }


        public static IQueryable<Models.Industry> Search(SizeUpContext context, string term)
        {
            var searchSpace = Base.Industry.Get(context)
                    .Select(i => new
                    {
                        Id = i.Id,
                        i.Name,
                        i.SEOKey,
                        SortOrder = 1
                    }).Union(Base.Industry.Keywords(context).Select(i => new
                    {
                        Id = i.IndustryId,
                        i.Name,
                        i.Industry.SEOKey,
                        i.SortOrder
                    }));

            foreach (var qs in term.Split(' '))
            {
                if (!string.IsNullOrWhiteSpace(qs))
                {
                    searchSpace = searchSpace.Where(i => i.Name.Contains(qs));
                }
            }

            var data = searchSpace
                .OrderBy(i => i.SortOrder)
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey
                });

            return data;
        }
    }
}
