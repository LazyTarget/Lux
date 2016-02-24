using System.Linq;

namespace Lux.Extensions
{
    public static class ArrayExtensions
    {
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
