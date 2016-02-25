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

        
        public static T[] Append<T>(this T[] array, T obj)
        {
            var objects = new T[] {obj};
            var res = Append<T>(array, objects);
            return res;
        }

        public static T[] Append<T>(this T[] array, params T[] objects)
        {
            var arr = (array ?? new T[0]);
            if (objects == null)
                return arr;
            var res = arr.Concat(objects).ToArray();
            return res;
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
