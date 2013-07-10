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
            var all = Base.Industry.Get(context);
            var raw = Base.Industry.GetActive(context);
            var naics = Base.Industry.GetNAICS(context);
            var data = raw
                .Where(i => i.Id == id)
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey,
                    ParentName = all.Where(p => p.SicCode == i.SicCode.Substring(0, i.SicCode.Length - 2)).Select(p => p.Name).FirstOrDefault(),
                    NAICS6 = i.SicToNAICSMappings.Select(n=> new Models.NAICS
                    {
                        Id = n.NAICS.Id,
                        Name = n.NAICS.Name,
                        NAICSCode = n.NAICS.NAICSCode
                    }).FirstOrDefault(),
                    NAICS4 = naics.Where(n=>n.NAICSCode == i.SicToNAICSMappings.Select(m=>m.NAICS.NAICSCode).FirstOrDefault().Substring(0,4)).Select(n=> new Models.NAICS
                    {
                        Id = n.Id,
                        Name = n.Name,
                        NAICSCode = n.NAICSCode
                    }).FirstOrDefault()
                        
                }).FirstOrDefault();
            return data;
        }

        public static Models.Industry Get(SizeUpContext context, string SEOKey)
        {
            var all = Base.Industry.Get(context);
            var raw = Base.Industry.GetActive(context);
            var naics = Base.Industry.GetNAICS(context);
            var data = raw
                .Where(i => i.SEOKey == SEOKey)
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey,
                    ParentName = all.Where(p => p.SicCode == i.SicCode.Substring(0, i.SicCode.Length - 2)).Select(p => p.Name).FirstOrDefault(),
                    NAICS6 = i.SicToNAICSMappings.Select(n => new Models.NAICS
                    {
                        Id = n.NAICS.Id,
                        Name = n.NAICS.Name,
                        NAICSCode = n.NAICS.NAICSCode
                    }).FirstOrDefault(),
                    NAICS4 = naics.Where(n => n.NAICSCode == i.SicToNAICSMappings.Select(m => m.NAICS.NAICSCode).FirstOrDefault().Substring(0, 4)).Select(n => new Models.NAICS
                    {
                        Id = n.Id,
                        Name = n.Name,
                        NAICSCode = n.NAICSCode
                    }).FirstOrDefault()

                }).FirstOrDefault();
            if (data == null)
            {
                data = new Models.Industry();
            }
            return data;
        }

        public static Models.Industry GetLegacy(SizeUpContext context, string SEOKey)
        {
            var all = Base.Industry.Get(context);
            var raw = Base.Industry.GetActive(context);
            var naics = Base.Industry.GetNAICS(context);
            var data = raw
                .Where(i => i.LegacyIndustrySEOKeys.Any(l=>l.SEOKey == SEOKey))
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey,
                    ParentName = all.Where(p => p.SicCode == i.SicCode.Substring(0, i.SicCode.Length - 2)).Select(p => p.Name).FirstOrDefault(),
                    NAICS6 = i.SicToNAICSMappings.Select(n => new Models.NAICS
                    {
                        Id = n.NAICS.Id,
                        Name = n.NAICS.Name,
                        NAICSCode = n.NAICS.NAICSCode
                    }).FirstOrDefault(),
                    NAICS4 = naics.Where(n => n.NAICSCode == i.SicToNAICSMappings.Select(m => m.NAICS.NAICSCode).FirstOrDefault().Substring(0, 4)).Select(n => new Models.NAICS
                    {
                        Id = n.Id,
                        Name = n.Name,
                        NAICSCode = n.NAICSCode
                    }).FirstOrDefault()

                }).FirstOrDefault();
            if (data == null)
            {
                data = new Models.Industry();
            }
            return data;
        }


        public static List<Models.Industry> Get(SizeUpContext context, List<long> industryIds)
        {
            var all = Base.Industry.Get(context);
            var raw = Base.Industry.GetActive(context);
            var naics = Base.Industry.GetNAICS(context);
            var data = raw
                .Where(i => industryIds.Contains(i.Id))
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey,
                    ParentName = all.Where(p => p.SicCode == i.SicCode.Substring(0, i.SicCode.Length - 2)).Select(p => p.Name).FirstOrDefault(),
                    NAICS6 = i.SicToNAICSMappings.Select(n => new Models.NAICS
                    {
                        Id = n.NAICS.Id,
                        Name = n.NAICS.Name,
                        NAICSCode = n.NAICS.NAICSCode
                    }).FirstOrDefault(),
                    NAICS4 = naics.Where(n => n.NAICSCode == i.SicToNAICSMappings.Select(m => m.NAICS.NAICSCode).FirstOrDefault().Substring(0, 4)).Select(n => new Models.NAICS
                    {
                        Id = n.Id,
                        Name = n.Name,
                        NAICSCode = n.NAICSCode
                    }).FirstOrDefault()

                })
                .ToList();
            return data;
        }

        public static List<Models.Industry> ListInPlace(SizeUpContext context, long placeId)
        {
            var all = Base.Industry.Get(context);
            var raw = Base.Industry.GetActive(context);
            var naics = Base.Industry.GetNAICS(context);
            var placeData = Base.IndustryData.City(context).Where(i => i.City.CityCountyMappings.Any(m => m.Id == placeId));
            var data = raw
                .Join(placeData, i=>i.Id, o=>o.IndustryId, (i,o)=> i)
                .Select(i => new Models.Industry
                {
                    Id = i.Id,
                    Name = i.Name,
                    SEOKey = i.SEOKey,
                    ParentName = all.Where(p => p.SicCode == i.SicCode.Substring(0, i.SicCode.Length - 2)).Select(p => p.Name).FirstOrDefault(),
                    NAICS6 = i.SicToNAICSMappings.Select(n => new Models.NAICS
                    {
                        Id = n.NAICS.Id,                        
                        Name = n.NAICS.Name,
                        NAICSCode = n.NAICS.NAICSCode
                    }).FirstOrDefault(),
                    NAICS4 = naics.Where(n => n.NAICSCode == i.SicToNAICSMappings.Select(m => m.NAICS.NAICSCode).FirstOrDefault().Substring(0, 4)).Select(n => new Models.NAICS
                    {
                        Id = n.Id,
                        Name = n.Name,
                        NAICSCode = n.NAICSCode
                    }).FirstOrDefault()

                })
                .ToList();
            return data;
        }


        public static IQueryable<Models.Industry> Search(SizeUpContext context, string term)
        {
            var searchSpace = Base.Industry.GetActive(context)
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
