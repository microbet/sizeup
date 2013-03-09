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
    public class Division : Base.Base
    {
        public static IQueryable<Models.Division> Get(SizeUpContext context)
        {
            var data = Base.Division.Get(context)
                .Select(i => new Models.Division
                {
                    Id = i.Id,
                    RegionName = i.Region.Name,
                    Name = i.Name
                });
            return data;
        }
    }
}
