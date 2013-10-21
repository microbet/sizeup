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
    public static class County
    {
        public class Default : Projection<Data.County, Models.County>
        {
            public override Expression<Func<Data.County, Models.County>> Expression
            {
                get
                {
                    return (i => new Models.County
                    {

                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey,
                        LongName = i.GeographicLocation.LongName,
                        ShortName = i.GeographicLocation.ShortName
                    });
                }
            }
        }
    }
}
