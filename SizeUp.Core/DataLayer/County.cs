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
    public class County
    {
        public static IQueryable<Data.County> Get(SizeUpContext context)
        {
            return context.Counties;
        }

        public static IQueryable<Models.County> InCity(SizeUpContext context, long cityId)
        {
            return context.Counties.Where(i => i.Cities.Any(c => c.Id == cityId)).Select(new Projections.County.Default().Expression);
        }
    }
}
