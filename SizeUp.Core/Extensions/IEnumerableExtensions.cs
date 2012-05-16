using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeUp.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<List<T>> InSetsOf<T>(this IEnumerable<T> source, int max)
        {
            List<T> toReturn = new List<T>(max);
            foreach (var item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == max)
                {
                    yield return toReturn;
                    toReturn = new List<T>(max);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }

        public static IEnumerable<List<T>> NTile<T, R>(this IEnumerable<T> source, Func<T, R> selector, int bands)
        {
            source = source.OrderBy(selector);
            int count = source.Count();
            int itemsPerBand = (int)Math.Ceiling((decimal)count / bands);
            List<T> toReturn = new List<T>(itemsPerBand);
            foreach (var item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == itemsPerBand)
                {
                    yield return toReturn;
                    toReturn = new List<T>(itemsPerBand);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }
    }

}
