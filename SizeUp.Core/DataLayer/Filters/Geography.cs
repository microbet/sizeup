using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.DataLayer.Filters
{
    public static class Geography
    {
        public class Calculation : Filter<Data.Geography>
        {
            public override Expression<Func<Data.Geography, bool>> Expression
            {
                get
                {
                    return i => i.GeographyClass.Name == Core.Geo.GeographyClass.Calculation;
                }
            }
        }

        public class Display : Filter<Data.Geography>
        {
            public override Expression<Func<Data.Geography, bool>> Expression
            {
                get
                {
                    return i => i.GeographyClass.Name == Core.Geo.GeographyClass.Display;
                }
            }
        }
    }
}
