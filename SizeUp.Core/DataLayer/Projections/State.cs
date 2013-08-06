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
    public static class State
    {
        public class Default : Projection<Data.State, Models.State>
        {
            public override Expression<Func<Data.State, Models.State>> Expression
            {
                get
                {
                    return (i => new Models.State
                    {

                        Id = i.Id,
                        Abbreviation = i.Abbreviation,
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
