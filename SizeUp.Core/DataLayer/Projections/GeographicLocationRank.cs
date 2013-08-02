using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;

namespace SizeUp.Core.DataLayer.Projections
{
    public static class GeographicLocationRank
    {
        public class Default : Projection<KeyValue<Models.Industry, long>, Models.GeographicLocationRank>
        {
            public override Expression<Func<KeyValue<Models.Industry, long>, Models.GeographicLocationRank>> Expression
            {
                get
                {
                    return (i => new Models.GeographicLocationRank
                    {
                        Rank = i.Value,
                        Industry = i.Key,
                    });
                }
            }
        }
    }
}
