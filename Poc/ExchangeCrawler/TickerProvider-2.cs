using PnlTickerDispatcher;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCrawler.ExchangeImplementation
{
    public class TickerProvider
    {

        public void Start()
        {
	 var exchanges = new ExchangeBuilder().BuildExchange();
	 foreach (var exchange in exchanges)
	 {
	     exchange.Start();
	 }
        }
    }

    public class ExchangeBuilder
    {
        public List<Exchange> BuildExchange()
	 => new() { new("BNCE"), new("BNSP"), new("HUBI"), new("CFUT") };
    }

    public class Exchange
    {
        private readonly TickerService tickerService;
        public Exchange(string name)
        {
	 Name = name;
	 tickerService = new();
        }
        public string Name { get; set; }

        public void Start()
        {
	 var pairs = tickerService.GetPairs();
	 var tickers = tickerService.GetTicker(pairs, Name);
        }

    }

    public class TickerService
    {
        public List<string> GetPairs()
        {
	 int pairCount = new Random().Next(1, 10000);
	 var list = Enumerable.Range(1, pairCount).ToList();
	 var pairList = new List<string>(list.Count);
	 foreach (var number in list)
	 {
	     pairList.Add($"Pair_{number}");
	 }
	 return pairList;
        }
       
        public Dictionary<string, Ticker> GetTicker(string pair, string exchange)
        {
	 return new Dictionary<string, Ticker> { { pair, new Ticker(exchange, pair) } };
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

    }

    public class Ticker
    {
        private static Random random = new Random();

        [SetsRequiredMembers]
        public Ticker(string exchange, string pair)
	 => (Exchange, Pair) = (exchange, pair);

        public required string Exchange { get; set; }
        public required string Pair { get; set; }
        public required double Bid { get; set; } = random.NextDouble();
        public required double Ask { get; set; } = random.NextDouble();
        public required double High { get; set; } = random.NextDouble();
        public required double Low { get; set; } = random.NextDouble();
        public required double Open { get; set; } = random.NextDouble();
        public required double Close { get; set; } = random.NextDouble();

    }

}
