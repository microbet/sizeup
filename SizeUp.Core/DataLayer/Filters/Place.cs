using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class Place 
    {
        public class Active : Filter<Data.Place>
        {
            public override Expression<Func<Data.Place, bool>> Expression
            {
                get
                {
                    return i => i.City.CityType.IsActive && i.City.IsActive;
                }
            }
        }
    }
}
