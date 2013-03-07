using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
using System.Linq.Expressions;
using SizeUp.Core.Extensions;
using SizeUp.Core.DataLayer.Models.Base;

namespace SizeUp.Core.DataLayer
{
    public class ConsumerExpenditures
    {
        public static IQueryable<Models.ConsumerExpenditureVariable> Variables(SizeUpContext context)
        {
            var data = Base.ConsumerExpenditures.Variables(context)
                .Select(i => new Models.ConsumerExpenditureVariable
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Variable = i.Variable,
                    Description = i.Description,
                    HasChildren = context.ConsumerExpenditureVariables.Where(c => c.ParentId == i.Id).Count() > 0
                });
            return data;
        }

        public static Models.ConsumerExpenditureVariable Variable(SizeUpContext context, long id)
        {
            return Variables(context).Where(i => i.Id == id).FirstOrDefault();
        }

        public static Models.ConsumerExpenditureVariable VariableCrosswalk(SizeUpContext context, long id)
        {
            var current = Variables(context).Where(i=>i.Id == id).Select(i=>i.Variable);
            return Variables(context)
                .Where(i => i.Variable == (current.FirstOrDefault().Substring(0, 1) == "X" ? "T" + current.FirstOrDefault().Substring(1) : "X" + current.FirstOrDefault().Substring(1)))
                .FirstOrDefault();
        }

        public static List<Models.ConsumerExpenditureVariable> VariablePath(SizeUpContext context, long id)
        {
            /* 
            i know what youre thinking.... and you are absolutely correct. I should be shot/hung/beaten/tortured/unthinkable
            i hated myself while i did this
            i wanted to commit sepiku
            i died a little inside
            it does work...and its not unbearibly slow
            so unless something comes to light that this is breaking the server....ill leave as is
            trying to pretend that it was only a dream
            hoping noone will notice
            wishing there was another way (without stored sprocs)
            OMG LOOK UNICORNS!
            arent you supposed to be looking at some other method?
            i think so....
            nothing to see here, move along
                        
            */
            List<Models.ConsumerExpenditureVariable> vars = new List<Models.ConsumerExpenditureVariable>();
            var v = Variable(context, id);
            vars.Add(v);
            while (v.ParentId != null)
            {
                v = Variables(context).Where(i => i.Id == v.ParentId).FirstOrDefault();
                vars.Add(v);
            }

            vars.Reverse();

            return vars;

        }

        public static List<Models.Band<long>> Bands(SizeUpContext context, long variableId, long placeId, int bands, Granularity granularity, Granularity boundingGranularity)
        {
            var variable = Variable(context, variableId);

            var defaultData = Base.ConsumerExpenditures.ZipCode(context).Where(i => 1 == 0);
            ConstantExpression constant = Expression.Constant(defaultData); //empty set
            Type dataType = typeof(ConsumerExpendituresByZip);
            IQueryProvider provider = defaultData.Provider;

            if (granularity == Granularity.ZipCode)
            {
                var entities = ZipCode.In(context, placeId, boundingGranularity);
                var data = entities.Join(Base.ConsumerExpenditures.ZipCode(context), o => o.Id, i => i.ZipCodeId, (i, o) => o);
                provider = data.Provider;
                dataType = typeof(ConsumerExpendituresByZip);
                constant = Expression.Constant(data);
            }
            else if (granularity == Granularity.City)
            {
                var entities = City.In(context, placeId, boundingGranularity);
                var data = entities.Join(Base.ConsumerExpenditures.City(context), o => o.Id, i => i.CityId, (i, o) => o);
                provider = data.Provider;
                dataType = typeof(ConsumerExpendituresByCity);
                constant = Expression.Constant(data);
            }
            else if (granularity == Granularity.County)
            {
                var entities = County.In(context, placeId, boundingGranularity);
                var data = entities.Join(Base.ConsumerExpenditures.County(context), o => o.Id, i => i.CountyId, (i, o) => o);
                provider = data.Provider;
                dataType = typeof(ConsumerExpendituresByCounty);
                constant = Expression.Constant(data);
            }
            else if (granularity == Granularity.State)
            {
                var entities = State.In(context, placeId, boundingGranularity);
                var data = entities.Join(Base.ConsumerExpenditures.State(context), o => o.Id, i => i.StateId, (i, o) => o);
                provider = data.Provider;
                dataType = typeof(ConsumerExpendituresByState);
                constant = Expression.Constant(data);
            }

            var param = Expression.Parameter(dataType, "c");
            var selector = Expression.Convert(Expression.Property(param, dataType.GetProperty(variable.Variable)), typeof(long?)) as Expression;         
            var pred = Expression.Lambda(selector, param) as Expression;
            var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, typeof(long?) }, constant, pred);
            

            var output = provider.CreateQuery<long?>(expression)
                .ToList()
                .NTile(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            Band<long> old = null;
            foreach (var band in output)
            {
                if (old != null)
                {
                    old.Max = band.Min;
                }
                old = band;
            }
            return output;
        }


    }
}
