namespace Crypto.Producer;

public class TickerPublisherService : ServiceBase, IService
{
	private readonly Publisher _publisher;
	private readonly TickerFaçade _tickerFaçade;
	private readonly List<string> _symbols;

	public TickerPublisherService(ILogger<IService> logger, TickerFaçade tickerFaçade, Publisher publisher, AppSetting appSetting)
		: base(appSetting, logger)
	{
		_tickerFaçade = tickerFaçade;
		_publisher = publisher;
		_symbols = appSetting.AppStartupSetting.EnabledExchanges["Binance"];
	}

	protected override async Task Execute()
	{
		var cts = new CancellationTokenSource();
		await FetchAndPublishMessageBag(cts.Token);
	}

	private async Task FetchAndPublishMessageBag(CancellationToken token)
	{
		_logger.LogInformation("Execution Started");

		if (_symbols == null || _symbols.Count == 0 || _symbols.Contains("all", StringComparer.OrdinalIgnoreCase))
		{
			var response = await _tickerFaçade.GetTickersAsync(token).ConfigureAwait(false);
			var message = new MessageBag<Ticker[]>(response.Response);
			var published = await _publisher.PublishAsync(message, token).ConfigureAwait(false);
		}
		else
		{
			foreach (var symbol in _symbols)
			{
				var response = await _tickerFaçade.GetTickerAsync(symbol, token).ConfigureAwait(false);
				var message = new MessageBag<Ticker[]>(new Ticker[] { response.Response });
				var published = await _publisher.PublishAsync(message, token).ConfigureAwait(false);
			}
		}
	}

	private async Task FetchAndPublishTickerBag(CancellationToken token)
	{
		_logger.LogInformation("Execution Started");

		if (_symbols == null || _symbols.Count == 0 || _symbols.Contains("all", StringComparer.OrdinalIgnoreCase))
		{
			var response = await _tickerFaçade.GetTickersAsync(token).ConfigureAwait(false);
			var message = new TickerBag(new Tickers(response.Response));
			var published = await _publisher.PublishAsync(message, token).ConfigureAwait(false);
		}
		else
		{
			foreach (var symbol in _symbols)
			{
				var response = await _tickerFaçade.GetTickerAsync(symbol, token).ConfigureAwait(false);
				var message = new TickerBag(new Tickers(response.Response));
				var published = await _publisher.PublishAsync(message, token).ConfigureAwait(false);
			}
		}
	}

	private async Task<bool> PublishTicker(TickerBag tickers, CancellationToken token)
	{
		var published = await _publisher.PublishAsync(tickers, token).ConfigureAwait(false);
		return published;
	}



	//  protected async Task Execute_Example()
	//  {
	//      _logger.LogInformation("Execution Started");
	//      var cts = new CancellationTokenSource();
	//      int option = 3;
	//      switch (option)
	//      {
	//case 0:
	//    var tickers = await _tickerFaçade.GetTickersAsync(cts.Token).ConfigureAwait(false);
	//    var published = await _publisher.PublishAsync(tickers, cts.Token).ConfigureAwait(false);
	//    break;
	//case 1:
	//    Task tickerTask = Task.Run(async () => { await _tickerFaçade.GetTickersAsync(cts.Token).ConfigureAwait(false); });
	//    await tickerTask.ContinueWith(tickers => { _ = _publisher.PublishAsync(tickers, cts.Token); }, TaskContinuationOptions.OnlyOnRanToCompletion).ConfigureAwait(false);
	//    break;
	//case 2:
	//    await _tickerFaçade.GetTickersAsync(cts.Token)
	//        .ContinueWith(tickers => { _ = _publisher.PublishAsync(tickers, cts.Token); }, TaskContinuationOptions.OnlyOnRanToCompletion)
	//        .ConfigureAwait(false);

	//    break;
	//case 3:
	//    var allTickers = await _tickerFaçade.GetTickerAsync("BTCUSDT", cts.Token).ConfigureAwait(false);
	//    _ = Task.Run(() =>
	//    {
	//        //TickerBag tickerBag = new (allTickers.Response);
	//        //Tickers tickers = Translate(allTickers);
	//        _publisher.PublishAsync(new TickerBag(new Ticker[] { allTickers.Response }), cts.Token)
	//        .ContinueWith(abc => { _logger.LogInformation("Execution Ended"); })
	//        .ConfigureAwait(false);

	//    });
	//    break;
	//      }
	//  }
	//  private async Task<MessageBag<Tickers>> FetchTicker(CancellationToken token)
	//  {
	//      _logger.LogInformation("Execution Started");
	//      Tickers tickers = new();

	//      if (_symbols == null || _symbols.Count == 0 || _symbols.Contains("all", StringComparer.OrdinalIgnoreCase))
	//      {
	//var response = await _tickerFaçade.GetTickersAsync(token).ConfigureAwait(false);
	//tickers.AddRange(response.Response);
	//      }
	//      else
	//      {
	//foreach (var symbol in _symbols)
	//{
	//    var ticker = await _tickerFaçade.GetTickerAsync(symbol, token).ConfigureAwait(false);
	//    tickers.Add(ticker.Response);
	//}
	//      }
	//      return new MessageBag<Tickers>(tickers);
	//  }
}