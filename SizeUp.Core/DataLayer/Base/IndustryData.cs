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
    public class IndustryData : Base
    {
        public static IQueryable<IndustryDataByZip> ZipCode(SizeUpContext context)
        {
            var data = context.IndustryDataByZips
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByCity> City(SizeUpContext context)
        {
            var data = context.IndustryDataByCities
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByCounty> County(SizeUpContext context)
        {
            var data = context.IndustryDataByCounties
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByMetro> Metro(SizeUpContext context)
        {
            var data = context.IndustryDataByMetroes
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByState> State(SizeUpContext context)
        {
            var data = context.IndustryDataByStates
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }

        public static IQueryable<IndustryDataByNation> Nation(SizeUpContext context)
        {
            var data = context.IndustryDataByNations
                       .Where(d => d.Year == Year && d.Quarter == Quarter);
            return data;
        }



    }
}
