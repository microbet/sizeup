using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Core.DataLayer.Models;


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

        public static IEnumerable<List<T>> NTileDescending<T, R>(this IEnumerable<T> source, Func<T, R> selector, int bands)
        {
            if (bands < 1)
            {
                bands = 1;
            }
            var groups = source.OrderByDescending(selector).GroupBy(selector);
            int totalItems = source.Count();
            int distinctCount = groups.Count();
            int itemsPerBand = Math.Max((int)Math.Floor((decimal)totalItems / (decimal)bands), 1);
            int bandIndex = 0;
            int distinctIndex = 0;
            List<T> toReturn = new List<T>();
            foreach (var group in groups)
            {
                distinctIndex++;
                int remainingBands = bands - bandIndex;
                int remainingValues = distinctCount - distinctIndex;
                toReturn.AddRange(group);
                //if bucket isnt empty and current group wont fit  OR  there arent enough remaining distinct values to fill all bands we want 
                if (toReturn.Count() >= itemsPerBand || remainingBands > remainingValues)
                {
                    yield return toReturn;
                    toReturn = new List<T>();
                    bandIndex++;
                }
                
            }
        }

        public static IEnumerable<List<T>> NTile<T, R>(this IEnumerable<T> source, Func<T, R> selector, int bands)
        {
            if (bands < 1)
            {
                bands = 1;
            }
            var groups = source.OrderBy(selector).GroupBy(selector);
            int totalItems = source.Count();
            int distinctCount = groups.Count();
            int itemsPerBand = Math.Max((int)Math.Floor((decimal)totalItems / (decimal)bands), 1);
            int bandIndex = 0;
            int distinctIndex = 0;
            List<T> toReturn = new List<T>();
            foreach (var group in groups)
            {
                distinctIndex++;
                int remainingBands = bands - bandIndex;
                int remainingValues = distinctCount - distinctIndex;
                toReturn.AddRange(group);
                //if bucket isnt empty and current group wont fit  OR  there arent enough remaining distinct values to fill all bands we want 
                if (toReturn.Count() >= itemsPerBand || remainingBands > remainingValues)
                {
                    yield return toReturn;
                    toReturn = new List<T>();
                    bandIndex++;
                }

            }
        }

        public static void FormatDescending<T>(this IEnumerable<Band<T>> source)
        {
            Band<T> old = null;
            foreach (Band<T> band in source)
            {
                if (old != null)
                {
                    old.Min = band.Max;
                }
                old = band;
            }
        }

        public static void Format<T>(this IEnumerable<Band<T>> source)
        {
            source.Reverse();
            Band<T> old = null;
            foreach (Band<T> band in source)
            {
                if (old != null)
                {
                    old.Min = band.Max;
                }
                old = band;
            }
            source.Reverse();
        }
     
    }

}
