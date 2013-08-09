using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
using System.Linq.Expressions;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class ConsumerExpenditures
    {
        public static IQueryable<Data.ConsumerExpenditureVariable> Variables(SizeUpContext context)
        {
            var data = context.ConsumerExpenditureVariables;
            return data;
        }

        public static IQueryable<Data.ConsumerExpenditure> Get(SizeUpContext context)
        {
            var data = context.ConsumerExpenditures.Where(i => i.Year == CommonFilters.TimeSlice.ConsumerExpenditures.Year && i.Quarter == CommonFilters.TimeSlice.ConsumerExpenditures.Quarter);
            return data;
        }


        public static Models.ConsumerExpenditureVariable Variable(SizeUpContext context, long id)
        {
            return Variables(context)
                .Where(i => i.Id == id)
                .Select(new Projections.ConsumerExpenditureVariable.Default().Expression)
                .FirstOrDefault();
        }

        public static IQueryable<Models.ConsumerExpenditureVariable> Variables(SizeUpContext context, long? parentId)
        {
            return Variables(context)
                .Where(i => i.ParentId == parentId)
                .Select(new Projections.ConsumerExpenditureVariable.Default().Expression);
        }

        public static Models.ConsumerExpenditureVariable VariableCrosswalk(SizeUpContext context, long id)
        {
            var current = Variables(context).Where(i=>i.Id == id).Select(i=>i.Variable);
            return Variables(context)
                .Where(i => i.Variable == (current.FirstOrDefault().Substring(0, 1) == "X" ? "T" + current.FirstOrDefault().Substring(1) : "X" + current.FirstOrDefault().Substring(1)))
                .Select(new Projections.ConsumerExpenditureVariable.Default().Expression)
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
            List<Data.ConsumerExpenditureVariable> vars = new List<Data.ConsumerExpenditureVariable>();
            var v = Variables(context).Where(i=>i.Id == id).FirstOrDefault();
            while (v != null)
            {
                vars.Add(v);
                v = v.Parent;
            }
            return vars.AsQueryable().Select(new Projections.ConsumerExpenditureVariable.Default().Expression).Reverse().ToList();
        }

        public static List<Models.Band<long>> Bands(SizeUpContext context, long variableId, long boundingGeographicLocationId, int bands, Granularity granularity)
        {
            var variable = Variables(context).Where(i=>i.Id == variableId).Select(i=>i.Variable).FirstOrDefault();
            var gran = Enum.GetName(typeof(Granularity), granularity);
            var data = Get(context)
                .Where(i => i.GeographicLocation.Granularity.Name == gran)
                .Where(i => i.GeographicLocation.GeographicLocations.Any(g => g.Id == boundingGeographicLocationId));

            ConstantExpression constant = Expression.Constant(data); //empty set
            Type dataType = typeof(ConsumerExpenditure);
            IQueryProvider provider = data.Provider;
            var param = Expression.Parameter(dataType, "c");
            var selector = Expression.Convert(Expression.Property(param, dataType.GetProperty(variable)), typeof(long?)) as Expression;         
            var pred = Expression.Lambda(selector, param) as Expression;
            var expression = Expression.Call(typeof(Queryable), "Select", new Type[] { dataType, typeof(long?) }, constant, pred);
            

            var output = provider.CreateQuery<long?>(expression)
                .Where(i => i != null)
                .ToList()
                .NTileDescending(i => i, bands)
                .Select(i => new Band<long>() { Min = i.Min(v => v.Value), Max = i.Max(v => v.Value) })
                .ToList();

            output.FormatDescending();
            return output;
        }


    }
}
