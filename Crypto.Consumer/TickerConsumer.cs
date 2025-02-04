namespace Crypto.Consumer;

public class TickerConsumer
{
	private readonly AppStartupSetting _appStartupSetting;

	private TickerBag _prevTickerBag;
	private MessageBag<Ticker[]> _currentTickerBag;
	private readonly Table _tickerTable;

	public TickerConsumer(AppStartupSetting appStartupSetting)
	{
		_appStartupSetting = appStartupSetting;
		_tickerTable = CreateTable();
		PrintTable(_tickerTable);
	}

	public async void NewTickerArrived(object? sender, MessageBag<Ticker[]> tickerBag)
	{
		_currentTickerBag = tickerBag;
	}

	private Table CreateTable()
	{
		var ticker = typeof(Ticker);
		var props = ticker.GetProperties();
		var table = new Table().Expand().BorderColor(Color.Grey);
		table.AddColumn($"[yellow]seq#[/]");
		table.AddColumn($"[yellow]symbol[/]");
		table.AddColumn($"[yellow]bidPrice[/]");
		table.AddColumn($"[yellow]bidQty[/]");
		table.AddColumn($"[yellow]askPrice[/]");
		table.AddColumn($"[yellow]askQty[/]");
		AnsiConsole.MarkupLine("Press [yellow]CTRL+C[/] to exit");
		return table;
	}
	private async Task PrintTable(Table tickerTable)
	{
		//tickerBag = new MessageBag<Ticker[]>(tickerBag.Message.Take(25).ToArray());
		try
		{
			await AnsiConsole.Live(tickerTable)
				.AutoClear(false)
				.Overflow(VerticalOverflow.Visible)
				.Cropping(VerticalOverflowCropping.Bottom)
				.StartAsync(ConstructTable(tickerTable));
		}
		catch (Exception ex)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
				Debugger.Log(0, "Consumer", ex.Message);
			}
		}
	}

	private Func<LiveDisplayContext, Task> ConstructTable(Table tickerTable)
	{
		return async ctx =>
		{
			//AddTickerRow(1,tickerBag.Message.First(), tickerTable);
			// Continuously update the table
			while (true)
			{
				if (_currentTickerBag != null && _currentTickerBag.Message.Count() > 0)
				{
					// More rows than we want?
					if (tickerTable.Rows.Count > _currentTickerBag.Message.Length)
						// Remove the first one
						for (int position = 0; position < tickerTable.Rows.Count; position++)
							tickerTable.Rows.RemoveAt(position);
					var seq = 0;
					// Add a new row
					foreach (var ticker in _currentTickerBag.Message)
						AddTickerRow(seq++, ticker, tickerTable);
					_prevTickerBag = new TickerBag(_currentTickerBag?.Message).Clone();
				}
				// Refresh and wait for a while
				ctx.Refresh();
				await Task.Delay(_appStartupSetting.IterationFrequencySec);

			}
		};
	}

	private void AddTickerRow(int seq, Ticker ticker, Table tickerTable)
	{
		try
		{
			var prevTicker = _prevTickerBag?.Message.FirstOrDefault(x => x.symbol.Equals(ticker.symbol));
			var bidPriceColumn = ticker.bidPrice < prevTicker?.bidPrice ? $"[red]{ticker.bidPrice}[/]" : $"[green]{ticker.bidPrice}[/]";
			var bidQtyColumn = ticker.bidQty < prevTicker?.bidQty ? $"[red]{ticker.bidQty}[/]" : $"[green]{ticker.bidQty}[/]";
			var askPriceColumn = ticker.askPrice < prevTicker?.askPrice ? $"[red]{ticker.askPrice}[/]" : $"[green]{ticker.askPrice}[/]";
			var askQtyColumn = ticker.askQty < prevTicker?.askQty ? $"[red]{ticker.askQty}[/]" : $"[green]{ticker.askQty}[/]";

			tickerTable.AddRow(seq.ToString(), $"[yellow]{ticker.symbol}[/]", bidPriceColumn, bidQtyColumn, askPriceColumn, askQtyColumn);

		}
		catch (Exception ex)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
				Debugger.Log(0, "Consumer", ex.Message);
			}
		}

	}
}

