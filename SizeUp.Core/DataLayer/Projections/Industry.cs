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
    public static class Industry
    {
        public class Default : Projection<Data.Industry, Models.Industry>
        {
            public override Expression<Func<Data.Industry, Models.Industry>> Expression
            {
                get
                {
                    return (i => new Models.Industry
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SEOKey = i.SEOKey,
                        SICCode = i.SicCode,
                        ParentName = i.Parent.Name,
                        NAICS6 = new Models.NAICS
                        {
                            Id = i.NAICS.Id,
                            NAICSCode = i.NAICS.NAICSCode,
                            Name = i.NAICS.Name
                        },
                        NAICS4 = new Models.NAICS
                        {
                            Id = i.NAICS.Parent.Id,
                            NAICSCode = i.NAICS.Parent.NAICSCode,
                            Name = i.NAICS.Parent.Name
                        }
                    });
                }
            }
        }
    }
}
