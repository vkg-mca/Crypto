namespace Crypto.Binance.Facades;

public class TickerFaçade
{
    private readonly IConnector _connector;
    private readonly ILogger<TickerFaçade> _logger;

    public TickerFaçade(ILogger<TickerFaçade> logger)
    {
        var httpClient = new HttpClient();
        var byte_connector = ConnectorFactory.GetInstance(MarketData.Environment.EnvVar.ConnectorProtocolType.Rest, MarketData.Environment.EnvVar.ConnectorResponseType.Byte, httpClient);
        var text_connector = ConnectorFactory.GetInstance(MarketData.Environment.EnvVar.ConnectorProtocolType.Rest, MarketData.Environment.EnvVar.ConnectorResponseType.Text, httpClient);
        _connector = byte_connector;
        _connector.BaseUrl = "https://api.binance.com";
        _connector.ApiKey = new KeyValue<string, string>("X-MBX-APIKEY", default);
        _logger = logger;
    }
    #region Synchronous version

    public CommandResponse<Ticker[]> GetTickers()
    {
        ICommand<Ticker[]> command = new TickerCommand<Ticker[]>()
        {
	 Connector = _connector,
	 CommandRequest = new CommandRequest() { EndPoint = "/api/v3/ticker/bookTicker", Method = HttpMethod.Get },
        };

        command.Execute();
        return command.CommandResponse;
    }

    public CommandResponse<Ticker> GetTicker(string symbol)
    {
        ICommand<Ticker> command = new TickerCommand<Ticker>()
        {
	 Connector = _connector,
	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
        };

        command.Execute();
        return command.CommandResponse;
    }

    public CommandResponse<Ticker> GetTickerSync(string symbol)
    {
        ICommand<Ticker> command = new TickerCommand<Ticker>()
        {
	 Connector = _connector,
	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
        };

        var result = command.ExecuteSync();
        return result;
    }


    #endregion

    #region Asynchronous version

    public async Task<CommandResponse<Ticker[]>> GetTickersAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var command = new TickerCommand<Ticker[]>()
        {
	 Connector = _connector,
	 CommandRequest = new CommandRequest() { EndPoint = "/api/v3/ticker/bookTicker", Method = HttpMethod.Get },
        };

        await command.ExecuteAsync();
        return command.CommandResponse;
    }

    public async Task<CommandResponse<Ticker>> GetTickerAsync(string symbol, CancellationToken ct)
    {
        ICommand<Ticker> command = new TickerCommand<Ticker>()
        {
	 Connector = _connector,
	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
        };

        await command.ExecuteAsync();
        //return command.CommandResponse;
        return command.CommandResponse;
    }

    #endregion
}
