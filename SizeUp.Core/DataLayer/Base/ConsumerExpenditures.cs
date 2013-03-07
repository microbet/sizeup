using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models.Base;
using System.Data.Objects.DataClasses;

namespace SizeUp.Core.DataLayer.Base
{
    public class ConsumerExpenditures : Base
    {
        public static IQueryable<ConsumerExpendituresByZip> ZipCode(SizeUpContext context)
        {
            var data = context.ConsumerExpendituresByZips
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<ConsumerExpendituresByCity> City(SizeUpContext context)
        {
            var data = context.ConsumerExpendituresByCities
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<ConsumerExpendituresByCounty> County(SizeUpContext context)
        {
            var data = context.ConsumerExpendituresByCounties
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<ConsumerExpendituresByState> State(SizeUpContext context)
        {
            var data = context.ConsumerExpendituresByStates
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<ConsumerExpenditureVariable> Variables(SizeUpContext context)
        {
            var data = context.ConsumerExpenditureVariables;                    
            return data;
        }


    }
}
