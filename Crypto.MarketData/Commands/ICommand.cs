namespace Crypto.MarketData.Commands;

public interface ICommand<T>
{
    IConnector Connector { get; set; }
    CommandRequest CommandRequest { get; set; }
    //CommandResponse CommandResponse { get; set; }
    CommandResponse<T> CommandResponse { get; set; }

    string ToString();

    void Execute() { throw new NotImplementedException(); }
    void Execute<TResponse>() where TResponse : CommandResponse { throw new NotImplementedException(); }
    CommandResponse<T> ExecuteSync();

    Task ExecuteAsync() { throw new NotImplementedException(); }
    Task ExecuteAsync<TResponse>() where TResponse : CommandResponse { throw new NotImplementedException(); }
}
