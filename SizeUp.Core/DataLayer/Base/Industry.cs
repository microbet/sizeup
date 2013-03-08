using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;

namespace SizeUp.Core.DataLayer.Base
{
    public class Industry : Base
    {
        public static IQueryable<Data.Industry> Get(SizeUpContext context)
        {
            var data = context.Industries;
            return data;
        }

        public static IQueryable<Data.Industry> GetActive(SizeUpContext context)
        {
            var data = context.Industries
                       .Where(d => d.IsActive);
            return data;
        }

        public static IQueryable<Data.IndustryKeyword> Keywords(SizeUpContext context)
        {
            var data = context.IndustryKeywords
                       .Where(d => d.Industry.IsActive);
            return data;
        }

        public static IQueryable<Data.NAICS> GetNAICS(SizeUpContext context)
        {
            var data = context.NAICS;
            return data;
        }
    }
}
