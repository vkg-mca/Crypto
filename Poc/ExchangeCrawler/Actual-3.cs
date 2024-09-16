using ExchangeCrawler.ExchangeImplementation;

namespace ExchangeCrawler.Actual
{
    public class TickerService
    {
        public void Start()
        {
	 var exchanges = LoadExchanges();

	 foreach (var exchange in exchanges)
	 {
	     exchange.Start();
	 }
        }
        private List<Exchange> LoadExchanges()
        {
	 return new()
	 {
	     new(){ Name="BNSP", BatchSize=50, PairCount=100, WaitSecondBeforeFetchNextBatch=1, TotalDurationSecond=30 },
	     new(){ Name="MAXX", BatchSize=50, PairCount=60, WaitSecondBeforeFetchNextBatch=2, TotalDurationSecond=30 },
	     new(){ Name="HUBI", BatchSize=50, PairCount=50, WaitSecondBeforeFetchNextBatch=3, TotalDurationSecond=30 }
	 };
        }
    }

    public class Exchange
    {
        private readonly Connector connector;
        public string Name { get; set; }
        public int BatchSize { get; set; } = 1;
        public int PairCount { get; set; }
        public int WaitSecondBeforeFetchNextBatch { get; set; }
        public int PairPerBatch { get { return PairCount / BatchSize; } }
        public int TotalDurationSecond { get; set; }
        public Exchange()
        {
	 connector = new Connector(Name);
        }

        public async void Start()
        {
	 while (true)
	 {
	     int fetchedTickerCount = 0;

	     var pairTask = Task.Run(connector.Command.GetPairs);
	     var pairs = pairTask.Result;

	     Console.WriteLine($"Exchange={Name}:Received {pairs.Count} Pairs");

	     int start = 0;
	     int end = BatchSize;
	     var range = new Range(start, end);

	     bool reset = false;
	     while (fetchedTickerCount <= PairCount)
	     {
	         var pairSegment = pairs.Take(range).ToList();

	         var tickerTask = Task.Run(()
		  =>
	         { return connector.Command.GetTicker(pairSegment, Name); });

	         var tickers = tickerTask.Result;
	         fetchedTickerCount += tickers.Count;
	         Console.WriteLine($"Exchange={Name}: Received {range.End.Value - range.Start.Value} Tickers[{range.Start}..{range.End}], Received so far={range.End.Value}");

	         if (fetchedTickerCount == PairCount)
	         {
		  //When PairCount is multiple of BatchSize
		  break;
	         }
	         else if (fetchedTickerCount + BatchSize > PairCount)
	         {
		  //When PairCount is not multiple of BatchSize
		  if (!reset)
		  {
		      end = PairCount;
		      start = end - (PairCount - fetchedTickerCount);
		      fetchedTickerCount = PairCount;
		      reset = true;
		  }
	         }
	         else
	         {
		  end = fetchedTickerCount + BatchSize;
		  start = fetchedTickerCount;
	         }
	         range = new Range(start, end);

	         Console.WriteLine($"Exchange={Name}:Waiting for {WaitSecondBeforeFetchNextBatch} seconds");
	         await Task.Delay(WaitSecondBeforeFetchNextBatch * 1000);

	     }

	 }
        }
    }

    public class Connector
    {
        public Connector(string name)
        {
	 Name = name;
	 Command = new(Name);
        }
        public string Name { get; set; }

        public Command Command { get; set; }
    }

    public class Command
    {
        public Command(string name)
        {
	 Name = name;
        }

        public string Name { get; set; }

        public List<string> GetPairs()
        {
	 int totalPairCount = 100;
	 int pairCount = new Random().Next(1, 10000);
	 var list = Enumerable.Range(1, totalPairCount).ToList();
	 var pairList = new List<string>(list.Count);
	 foreach (var number in list)
	 {
	     pairList.Add($"Pair_{number}");
	 }
	 return pairList;
        }

        public Dictionary<string, List<Ticker>> GetTicker(List<string> pairs, string exchange)
        {
	 Dictionary<string, List<Ticker>> tickerDict = new(pairs.Count);
	 foreach (var pair in pairs)
	 {
	     var ticker = GetTicker(pair, exchange);
	     if (tickerDict.TryGetValue(pair, out var tickers))
	     {
	         tickers.AddRange(ticker.Values);
	     }
	     else
	     {
	         tickerDict[pair] = new() { ticker[pair] };
	     }
	 }
	 return tickerDict;

        }
        public Dictionary<string, Ticker> GetTicker(string pair, string exchange)
        {
	 return new() { { pair, new(exchange, pair) } };
        }
    }

}
