namespace Lux.Tests.Model.Models
{
    public interface IBaseClass
    {
        string Foo { get; set; }
    }

    public interface IDerivedClass : IBaseClass
    {
        string Bar { get; set; }
    }
}