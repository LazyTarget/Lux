using NUnit.Framework;
using System.Collections.Generic;

namespace Lux.Tests.Serialization.Models
{
    public class PocoClass
    {
        public string StringProp { get; set; }
        public double DoubleProp { get; set; }
        public int IntProp { get; set; }
        public object ObjectProp { get; set; }
        public PocoClass PocoProp { get; set; }
        public List<PocoClass> PocoList { get; set; }
    }
}
