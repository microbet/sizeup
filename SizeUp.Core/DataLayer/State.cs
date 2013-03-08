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
    public class State : Base.Base
    {
        public static IQueryable<Models.State> Get(SizeUpContext context)
        {
            var data = Base.Place.Get(context)
                .Select(i => new Models.State
                {
                    Id = i.County.State.Id,
                    Abbreviation = i.County.State.Abbreviation,
                    Name = i.County.State.Name,
                    SEOKey = i.County.State.SEOKey
                })
                .Distinct();
            return data;
        }
    }
}
