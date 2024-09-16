namespace Crypto.MarketData.Connectors;

public interface IConnector
{
    string BaseUrl { get; set; }
    public KeyValue<string, string> ApiKey { get; set; }
    //void Execute() { throw new NotImplementedException(); }
    Task<T> SendAsync<T>(CommandRequest request);
}
