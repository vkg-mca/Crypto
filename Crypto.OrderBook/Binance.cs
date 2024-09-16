//namespace OrderBook;

//public class Binance
//{
//    private readonly IConnector _connector;

//    public Binance()
//    {
//        var httpClient = new HttpClient();
//        var byte_connector = ConnectorFactory.GetInstance(Crypto.MarketData.Environment.EnvVar.ConnectorProtocolType.Rest, Crypto.MarketData.Environment.EnvVar.ConnectorResponseType.Byte, httpClient);
//        var text_connector = ConnectorFactory.GetInstance(Crypto.MarketData.Environment.EnvVar.ConnectorProtocolType.Rest, Crypto.MarketData.Environment.EnvVar.ConnectorResponseType.Text, httpClient);
//        _connector = byte_connector;
//        _connector.BaseUrl = "https://api.binance.com";
//        _connector.ApiKey = new KeyValue<string, string>("X-MBX-APIKEY", default);

//    }

//    #region Synchronous version

//    public CommandResponse<Ticker[]> GetTickers()
//    {
//        ICommand<Ticker[]> command = new TickerCommand<Ticker[]>()
//        {
//	 Connector = _connector,
//	 CommandRequest = new CommandRequest() { EndPoint = "/api/v3/ticker/bookTicker", Method = HttpMethod.Get },
//        };

//        command.Execute();
//        return command.CommandResponse;
//    }

//    public CommandResponse<Ticker> GetTicker(string symbol)
//    {
//        ICommand<Ticker> command = new TickerCommand<Ticker>()
//        {
//	 Connector = _connector,
//	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
//        };

//        command.Execute();
//        return command.CommandResponse;
//    }

//    public CommandResponse<Ticker> GetTickerSync(string symbol)
//    {
//        ICommand<Ticker> command = new TickerCommand<Ticker>()
//        {
//	 Connector = _connector,
//	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
//        };

//        var result = command.ExecuteSync();
//        return result;
//    }


//    #endregion

//    #region Asynchronous version

//    public async Task<CommandResponse<Ticker[]>> GetTickersAsync()
//    {
//        var command = new TickerCommand<Ticker[]>()
//        {
//	 Connector = _connector,
//	 CommandRequest = new CommandRequest() { EndPoint = "/api/v3/ticker/bookTicker", Method = HttpMethod.Get },
//        };

//        await command.ExecuteAsync();
//        return command.CommandResponse;
//    }

//    public async Task<CommandResponse<Ticker>> GetTickerAsync(string symbol)
//    {
//        ICommand<Ticker> command = new TickerCommand<Ticker>()
//        {
//	 Connector = _connector,
//	 CommandRequest = new CommandRequest() { EndPoint = $"/api/v3/ticker/bookTicker?symbol={symbol}", Method = HttpMethod.Get },
//        };

//        await command.ExecuteAsync();
//        //return command.CommandResponse;
//        return command.CommandResponse;
//    }

//    #endregion



//}
