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
    public class State
    {
        public static IQueryable<Data.State> Get(SizeUpContext context)
        {
            return context.States;
        }

        public static IQueryable<Models.State> List(SizeUpContext context)
        {
            return Get(context).Select(new Projections.State.Default().Expression);
        }
    }
}
