namespace Crypto.Binance.Commands.Rest;

public class DepthCommand : RestCommand<Crypto.Domain.Depth>
{
    public DepthCommand()
    {
        //https://api.binance.com/api/v3/depth?symbol=BNBBTC&limit=1000
    }

    public override void Execute()
    {
        Task<Ticker[]> task = Connector.SendAsync<Ticker[]>(CommandRequest);
        var result = task.Result;

    }
}
