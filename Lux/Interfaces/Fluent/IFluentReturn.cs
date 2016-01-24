namespace Lux.Interfaces
{
    public interface IFluentReturn<out TReturn>
    {
        TReturn Return();
    }
}
