using System.Collections.Generic;

namespace Lux.IO
{
    public class PathSorter : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var c = PathHelper.Compare(x, y);
            if (c == 0)
                return 0;
            c = 0;

            var xParts = PathHelper.GetPathParts(x);
            var yParts = PathHelper.GetPathParts(y);
            var xPath = "";
            var yPath = "";
            var lengthC = xParts.Length.CompareTo(yParts.Length);
            for (var i = 0; true ; i++)
            {
                if (lengthC != 0)
                {
                    if (i >= xParts.Length)
                        return lengthC;
                    if (i >= yParts.Length)
                        return lengthC;
                }
                if (c != 0)
                    return c;

                xPath = PathHelper.Combine(xPath, xParts[i]);
                yPath = PathHelper.Combine(yPath, yParts[i]);

                c = PathHelper.Compare(xPath, yPath);
            }
            return 0;
        }
    }
}