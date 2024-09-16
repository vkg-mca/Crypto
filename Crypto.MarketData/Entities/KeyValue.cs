namespace Crypto.MarketData.Entities;

public class KeyValue<TKey, TValue>
{
    public KeyValue(TKey key, TValue value) => (Key, Value) = (key, value);

    public TKey Key { get; set; }

    public TValue Value { get; set; }
}
