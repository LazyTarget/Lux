using System.Collections.Generic;
using System.Linq;

namespace Lux
{
    public static class ListExtensions
    {
        public static IList<T> OrderBy<T>(this IList<T> list, IComparer<T> comparer)
        {
            var l = list.ToList();
            l.Sort(comparer);
            return l;
        }
    }
}
