namespace Lux.Tests.Model.Models
{
    public class BaseClass : IBaseClass
    {
        public string Foo { get; set; }
    }

    public class DerivedClass : BaseClass, IDerivedClass
    {
        public string Bar { get; set; }
    }
}
