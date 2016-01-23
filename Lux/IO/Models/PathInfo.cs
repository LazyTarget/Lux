using System;

namespace Lux.IO
{
    public class PathInfo : IEquatable<PathInfo>
    {
        private string _absolutePath;
        private string _relativePath;

        public PathInfo()
        {
            
        }

        public PathInfo(string absolutePath, string relativePath)
            : this()
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
        }

        public string AbsolutePath
        {
            get { return _absolutePath; }
            set { _absolutePath = PathHelper.Normalize(value); }
        }

        public string RelativePath
        {
            get { return _relativePath; }
            set { _relativePath = PathHelper.Normalize(value); }
        }


        public bool Equals(PathInfo other)
        {
            if (other == null)
                return false;
            var a = AbsolutePath.Equals(other.AbsolutePath);
            var b = RelativePath.Equals(other.RelativePath);
            return a && b;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is PathInfo)
                return Equals((PathInfo)obj);
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return AbsolutePath;
        }
    }
}
