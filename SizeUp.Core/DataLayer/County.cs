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
    public class County : Base.Base
    {
        public static IQueryable<Models.County> Get(SizeUpContext context)
        {
            var data = Base.Place.Get(context)
                .Select(i => new Models.County
                {
                    Id = i.County.Id,
                    Name = i.County.Name,
                    SEOKey = i.County.SEOKey
                })
                .Distinct();
            return data;
        }

        public static IQueryable<Models.County> GetInState(SizeUpContext context, long stateId)
        {
            var data = Base.Place.Get(context)
                .Where(i => i.County.StateId == stateId)
                .Select(i => new Models.County
                {
                    Id = i.County.Id,
                    Name = i.County.Name,
                    SEOKey = i.County.SEOKey
                })
                .Distinct();
            return data;
        }
    }
}
