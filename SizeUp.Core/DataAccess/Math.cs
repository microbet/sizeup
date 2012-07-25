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
        public enum Order
        {
            LessThan,
            GreaterThan
        }

        public static int? Percentile(IQueryable<double?> source, double value, Order order = Order.LessThan)
        {
            IQueryable<double?> filtered = null;
            if(order == Order.GreaterThan){
                filtered = source.Where(i => i.Value >= value);
            }
            else{
                filtered = source.Where(i => i.Value <= value);
            }

            var data = new
            {
                Total = source.Count(),
                Less = filtered.Count()
            };

            int? val = null;
            if (data.Total > 0)
            {
                val = (int)(((decimal)data.Less / (decimal)data.Total) * 100);
            }
            return val;
        }

        public static int? Percentile(IQueryable<long?> source, long value, Order order = Order.LessThan)
        {
            IQueryable<long?> filtered = null;
            if (order == Order.GreaterThan)
            {
                filtered = source.Where(i => i.Value >= value);
            }
            else
            {
                filtered = source.Where(i => i.Value <= value);
            }

            var data = new
            {
                Total = source.Count(),
                Less = filtered.Count()
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
