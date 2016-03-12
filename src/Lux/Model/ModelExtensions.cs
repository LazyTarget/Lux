using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Model
{
    public static class ModelExtensions
    {
        public static bool HasProperty(this IObjectModel obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            var property = obj.GetProperty(propertyName);
            var result = property != null;
            return result;
        }

        public static IEnumerable<string> GetPropertyNames(this IObjectModel obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            var result = obj.GetProperties().Select(x => x.Name);
            return result;
        }

    }
}
