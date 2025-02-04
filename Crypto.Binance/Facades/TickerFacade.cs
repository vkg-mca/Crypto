namespace Crypto.Binance.Facades;

public partial class TickerFaçade
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

}
