namespace Crypto.MarketData.Commands;

public class CommandResponse
{
    public int Code { get; set; }
    public string Message { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class CommandResponse<TResponse> : CommandResponse
{
    public TResponse Response { get; set; }
}
