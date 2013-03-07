using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
using System.Linq.Expressions;

namespace SizeUp.Core.DataLayer
{
    public class ConsumerExpenditures
    {
        public static ConsumerExpenditureVariable Variable(SizeUpContext context, int id)
        {
            return Base.ConsumerExpenditures.Variables(context).Where(i => i.Id == id).FirstOrDefault();
        }

        public static Models.Business Bands(SizeUpContext context, int variableId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var variable = Variable(context, variableId);

            var zip = Base.ConsumerExpenditures.ZipCode(context);
            var city = Base.ConsumerExpenditures.City(context);
            var county = Base.ConsumerExpenditures.County(context);
            var state = Base.ConsumerExpenditures.State(context);

    
            var param = Expression.Parameter(typeof(ConsumerExpendituresByCity), "c");
            var selector = Expression.Property(param, typeof(ConsumerExpendituresByCity).GetProperty(variable.Variable)) as Expression;
            var pred = Expression.Lambda(selector, param) as Expression;
    
            
            var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { typeof(ConsumerExpendituresByCity), typeof(long?) }, Expression.Constant(city), pred);
            
            var test = city.AsQueryable().Provider.CreateQuery<long?>(expression);
            
     

            return null;
        }


    }
}
