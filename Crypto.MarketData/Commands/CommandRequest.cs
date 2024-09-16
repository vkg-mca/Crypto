namespace Crypto.MarketData.Commands;

public class CommandRequest
{
    public string EndPoint { get; set; }
    public HttpMethod Method { get; set; }
    public object Content { get; set; }
}

public class CommandRequest<TRequest> : CommandRequest
{
    public TRequest? Request { get; set; }
}
