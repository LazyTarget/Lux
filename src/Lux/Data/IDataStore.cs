namespace Lux.Data
{
    public interface IDataStore<TKey>
    {
        object Load(TKey key);
        object Save(TKey key, object value);
    }

    public interface IDataStore<TKey, TValue> : IDataStore<TKey>
    {
        new TValue Load(TKey key);
        TValue Save(TKey key, TValue value);
    }
}