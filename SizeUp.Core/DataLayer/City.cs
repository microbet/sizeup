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
    public class City
    {
        public static IQueryable<Data.City> Get(SizeUpContext context)
        {
            return context.Cities;
        }
    }
}
