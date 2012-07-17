using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;

namespace SizeUp.Core.DataAccess
{
    public static class Math
    {
        public static int? Percentile(IQueryable<double?> source, double value)
        {
            var data = new
            {
                Total = source.Count(),
                Less = source.Where(i => i.Value <= value).Count()
            };

            int? val = null;
            if (data.Total > 0)
            {
                val = (int)(((decimal)data.Less / (decimal)data.Total) * 100);
            }
            return val;
        }

        public static int? Percentile(IQueryable<long?> source, long value)
        {
            var data = new
            {
                Total = source.Count(),
                Less = source.Where(i => i.Value <= value).Count()
            };

            int? val = null;
            if (data.Total > 0)
            {
                val = (int)(((decimal)data.Less / (decimal)data.Total) * 100);
            }
            return val;
        }
    }
}
