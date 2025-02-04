namespace Crypto.Binance.Facades;

public partial class TickerFaçade
{
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
}
