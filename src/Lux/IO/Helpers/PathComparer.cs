using System.Collections.Generic;

namespace Lux.IO
{
    public class PathComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var res = PathHelper.Compare(x, y);
            return res;
        }
    }
}
