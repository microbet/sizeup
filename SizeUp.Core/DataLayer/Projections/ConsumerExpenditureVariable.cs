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
    public static class ConsumerExpenditureVariable
    {
        public class Default : Projection<Data.ConsumerExpenditureVariable, Models.ConsumerExpenditureVariable>
        {
            public override Expression<Func<Data.ConsumerExpenditureVariable, Models.ConsumerExpenditureVariable>> Expression
            {
                get
                {
                    return i => new Models.ConsumerExpenditureVariable()
                    {
                        Id = i.Id,
                        ParentId = i.ParentId,
                        Variable = i.Variable,
                        Description = i.Description,
                        HasChildren = i.Children.Count() > 0
                    };
                }
            }
        }
    }
}
