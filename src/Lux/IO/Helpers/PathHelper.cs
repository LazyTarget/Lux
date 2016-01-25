using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lux.IO
{
    public static class PathHelper
    {
        public static string Normalize(string path)
        {
            var res = ( path ?? "" ).Trim()
                                    .Replace("\\\\", "\\")      // Replace \\ with \
                                    .Replace('\\', '/');        // Replace \ with /
            if (res.EndsWith(":"))
                res = res + "/";
            return res;
        }


        public static bool AreEquivalent(string x, string y)
        {
            var c = Compare(x, y);
            var res = c == 0;
            return res;
        }

        public static int Compare(string x, string y)
        {
            x = Normalize(x);
            y = Normalize(y);
            var res = x.CompareTo(y);
            return res;

            //x = Normalize(x);
            //y = Normalize(y);
            //if (x == y)
            //    return 0;
            //if (x.StartsWith(y))
            //    return 1;
            //return -1;
        }



        public static string Combine(params string[] paths)
        {
            var parts = paths;
            parts = parts.Select(Normalize).ToArray();

            string res;
            if (parts.Any(x => x.Any(c => Path.GetInvalidPathChars().Contains(c))))
                res = String.Join("/", paths.Select(x => x.TrimEnd('/')));
            else
                res = Path.Combine(parts);
            
            if (Path.IsPathRooted(res))
            {
                if (res.EndsWith(":"))
                    res = res + "/";

                var hasWildcards = res.Any(x => ( x == '*' || x == '?' ));
                if (!hasWildcards)
                {
                    var info = new DirectoryInfo(res);
                    res = info.FullName;
                }
            }
            res = Normalize(res);
            return res;
        }


        public static string GetParent(string path)
        {
            var res = GetParentOrDefault(path);
            if (string.IsNullOrEmpty(res))
                throw new Exception("Path as no parent");
            return res;
        }

        public static string GetParentOrDefault(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var res = path;
            if (res.EndsWith(":/"))
            {
                res = null;
            }
            else
            {
                res = Normalize(path);
                if (Path.IsPathRooted(res))
                {
                    if (res.EndsWith(":"))
                        res = res + "/";

                    var hasWildcards = res.Any(x => (x == '*' || x == '?'));
                    if (!hasWildcards)
                    {
                        var info = new DirectoryInfo(res);
                        if (info.Parent != null)
                            res = info.Parent.FullName;
                    }
                    res = Normalize(res);
                }
                else
                    res = null;
            }
            return res;
        }


        public static string[] GetPathParts(string path)
        {
            path = Normalize(path);
            var parts = path.Split('/');
            return parts;
        }


        public static string GetRootParent(string path)
        {
            //var parts = GetPathParts(path);
            //var res = parts.FirstOrDefault();
            //return res;

            string res;
            var tmp = path;
            do
            {
                res = tmp;
                tmp = GetParentOrDefault(tmp);
            } while (!string.IsNullOrEmpty(tmp));
            return res;
        }


        public static string Subtract(string x, string y)
        {
            x = Normalize(x);
            y = Normalize(y);
            var res = x;
            if (x.StartsWith(y))
            {
                var length = y.Length;
                if (x.StartsWith(y + "/"))
                    length++;
                res = x.Remove(0, length);
            }
            res = Normalize(res);
            return res;
        }


        public static bool ValidateSearchPattern(string name, string searchPattern)
        {
            var valid = string.IsNullOrEmpty(searchPattern) || searchPattern.All(x => x.Equals('*') || searchPattern == "*.*");
            if (valid)
                return valid;

            var patterns = searchPattern.Split('|');
            foreach (var pattern in patterns)
            {
                if (string.IsNullOrEmpty(pattern))
                    continue;
                var regexPattern = WildcardToRegex(pattern);
                valid = Regex.IsMatch(name, regexPattern);
                if (valid)
                    break;
            }
            return valid;
        }

        private static string WildcardToRegex(string pattern)
        {
            var res = "^" + Regex.Escape(pattern)
                                 .Replace(@"\*", ".*")
                                 .Replace(@"\?", ".") + "$";
            return res;
        }

    }
}
