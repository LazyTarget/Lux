namespace Lux.Config
{
    public interface IConfigArguments
    {
        IConfigLocation Location { get; set; }

        // todo: Constructor arguments/info?
        // todo: IDeserializer?, ISerializer?
    }
}
