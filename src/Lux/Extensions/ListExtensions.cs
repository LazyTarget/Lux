using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lux.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> OrderBy<T>(this IList<T> list, IComparer<T> comparer)
        {
            var l = list.ToList();
            l.Sort(comparer);
            return l;
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Expression<Func<TSource, TKey>> keySelector)
        {
            var func = keySelector.Compile();
            var result = DistinctBy(source, func);
            return result;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }


        public static int RemoveAll<T>(this IList<T> list, Expression<Func<T, bool>> predicate)
        {
            var removed = 0;
            var func = predicate.Compile();
            for (var index = 0; index < list.Count; index++)
            {
                var item = list.ElementAt(index);
                var shouldRemove = func(item);
                if (shouldRemove)
                {
                    list.RemoveAt(index);
                    index--;
                }
            }
            return removed;
        }

    }
}
