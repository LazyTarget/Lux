using System.Collections.Generic;
using System.Linq;

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

    }
}
