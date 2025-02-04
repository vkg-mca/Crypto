namespace Crypto.Binance.Facades;

public partial class TickerFaçade
{
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
}
