using System.Collections.Generic;

namespace Lux.IO
{
    public class SubPathComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            x = PathHelper.Normalize(x);
            y = PathHelper.Normalize(y);
            int res;
            if (x == y)
                res = 0;
            else if (x.StartsWith(y))
                res = 1;
            else
                res = -1;
            return res;
        }
    }
}
