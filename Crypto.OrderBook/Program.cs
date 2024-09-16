namespace Crypto.OrderBook;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await UseCode();
        //await UseClient();
    }

    private static async Task UseCode()
    {
        var cts = new CancellationTokenSource();
        DateTime dt1 = DateTime.Now;
        //var logger = new LogFactory().GetLogger("Default");
        TickerFaçade binance = new(default);

        var result1 = binance.GetTicker("ETHBTC");
        await Console.Out.WriteLineAsync(result1.ToJsonString());

        var result3 = await binance.GetTickerAsync("ETHBTC",cts.Token);
        await Console.Out.WriteLineAsync(result3.ToJsonString());

        var result5 = binance.GetTickerSync("ETHBTC");
        await Console.Out.WriteLineAsync(result5.ToJsonString());

        var result2 = binance.GetTickers();
        await Console.Out.WriteLineAsync(result2.ToJsonString());

        var result4 = await binance.GetTickersAsync(cts.Token);
        await Console.Out.WriteLineAsync(result4.ToJsonString());

        DateTime dt2 = DateTime.Now;
        await Console.Out.WriteLineAsync($"1={(dt2 - dt1).TotalMilliseconds}");
    }

    private static async Task UseClient()
    {
        DateTime dt1 = DateTime.Now;
        IClient client = new Client(new HttpClient());
        //var tikers = await client.TickerAsync("ETHBTC", null , "1d", "FULL");
        var tickers = await client.BookTickerAsync("ETHBTC", null);
        //var response = await client.DepthAsync("ETHBTC", 10);
        DateTime dt2 = DateTime.Now;
        await Console.Out.WriteLineAsync($"2={(dt2 - dt1).TotalMilliseconds}");
        await Console.Out.WriteLineAsync(tickers.ToJsonString());

    }
    //      private static void PrintHeader()
    //      {
    //var ticker = new Crypto.Binance.Entities.Ticker();
    //var headerTable = new Table()
    //   .Border(TableBorder.Square)
    //   .BorderColor(Color.Red)
    //   .AddColumn(new TableColumn($"[u][yellow]{nameof(ticker.symbol)}[/][/]")/*.Footer($"[yellow]{nameof(ticker.symbol)}[/]").Centered()*/)
    //   .AddColumn(new TableColumn($"[u][yellow]{nameof(ticker.askPrice)}[/][/]")/*.Footer($"[yellow]{nameof(ticker.askPrice)}[/]").Centered()*/)
    //   .AddColumn(new TableColumn($"[u][yellow]{nameof(ticker.bidPrice)}[/][/]")/*.Footer($"[yellow]{nameof(ticker.bidPrice)}[/]").Centered()*/)
    //   .AddColumn(new TableColumn($"[u][yellow]{nameof(ticker.askQty)}[/][/]")/*.Footer($"[yellow]{nameof(ticker.askQty)}[/]").Centered()*/)
    //   .AddColumn(new TableColumn($"[u][yellow]{nameof(ticker.bidQty)}[/][/]")/*.Footer($"[yellow]{nameof(ticker.bidQty)}[/]").Centered()*/);
    //AnsiConsole.Write(headerTable);
    //      }

    //      private static void PrintDetails()
    //      {
    //var detailTable = new Table()
    //    .AddColumn(new TableColumn(""))
    //    .AddColumn(new TableColumn(""))
    //    .AddColumn(new TableColumn(""))
    //    .AddColumn(new TableColumn(""))
    //    .AddColumn(new TableColumn(""));
    //foreach (var kv in _tickerDict)
    //{
    //    var ticker = kv.Value as Crypto.Binance.Entities.Ticker;
    //    detailTable.AddRow(ticker.symbol, ticker.askPrice.ToString(), ticker.bidPrice.ToString(), ticker.askQty.ToString(), ticker.bidQty.ToString());
    //}
    //AnsiConsole.Write(detailTable);
    //      }
}