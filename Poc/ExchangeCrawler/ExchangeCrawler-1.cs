using System.Collections.Concurrent;
using System.Timers;

namespace PnlTickerDispatcher.BatchImplementation
{
    public class ExchangeCrawlerService
    {
        public void Start()
        {
	 var factory = new TimerFactory();

	 ExchangeCrawlerConfig crawlerConfig = new ExchangeCrawlerConfig();
	 foreach (var runtimeUnit in crawlerConfig.RunTimeConfig.Config)
	 {
	     var crawler = new ExchangeCrawler(runtimeUnit, factory);
	     crawler.Crawl();
	 }
        }
    }

    public class ExchangeCrawler
    {
        private TimerFactory factory;
        private RunTimeConfigElement runtimeUnit;

        public ExchangeCrawler(RunTimeConfigElement runtimeUnit, TimerFactory factory)
        {
	 this.runtimeUnit = runtimeUnit;
	 this.factory = factory;
        }

        public void Crawl()
        {
	 var timer = factory.GetNewTimer(runtimeUnit.Exchange, runtimeUnit.DelaySecond);
	 timer.Elapsed += new ElapsedEventHandler(FetchAndPublishTickers);
	 timer.Enabled = true;
        }

        private int start = 0;
        private int count = 0;

        private void FetchAndPublishTickers(object? sender, ElapsedEventArgs e)
        {

	 if (start + runtimeUnit.BatchSize < runtimeUnit.PairCount)
	 {
	     count = runtimeUnit.BatchSize;

	 }
	 else
	 {
	     count = start + runtimeUnit.BatchSize > runtimeUnit.PairCount
	         ? runtimeUnit.PairCount - start
	         : runtimeUnit.BatchSize;

	 }

	 List<int> pairs = runtimeUnit.Pairs.GetRange(start, count);
	 Console.ForegroundColor = runtimeUnit.FontColor;
	 var timer = (sender as NamedTimer);
	 Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId},Timer={timer.Sequence},Interval={runtimeUnit.DelaySecond}Sec, Exchange={runtimeUnit.Exchange}, #Pairs={runtimeUnit.PairCount}, Count={count},Start={start}, End={start + count}, Pairs={pairs[0]}..{pairs[^1]}");

	 Fetch();
	 Publish();

	 start += count;
	 if (start >= runtimeUnit.PairCount)
	     start = 0;

        }
        private void Fetch() { }
        private void Publish() { }
    }

    public class ExchangeCrawlerConfig
    {
        public UpTimeConfig UpTimeConfig { get; set; }

        public RunTimeConfig RunTimeConfig { get { return new(); } }

    }

    public class UpTimeConfig
    {
    }

    public class RunTimeConfig
    {
        public List<RunTimeConfigElement> Config
        {
	 get
	 {
	     return new()
	     {
	         new () {Exchange="EXCH0", PairCount = 4271, BatchSize =430},
	         new () {Exchange="EXCH1", PairCount = 6782, BatchSize = 700},
	         new () {Exchange="EXCH2", PairCount = 501, BatchSize = 500},
	         new () {Exchange="EXCH3", PairCount = 301, BatchSize =31},
	         new () {Exchange="EXCH4", PairCount = 567, BatchSize =510},
	         new () {Exchange="EXCH5", PairCount = 5317, BatchSize =540},
	         new () {Exchange="EXCH6", PairCount = 10089, BatchSize =1100},
	         new () {Exchange="EXCH7", PairCount = 118, BatchSize =12},
	         new () {Exchange="EXCH8", PairCount = 432, BatchSize =45},
	         new () {Exchange="EXCH9", PairCount = 80, BatchSize =8},
	     };
	 }
        }
    }

    public class RunTimeConfigElement
    {
        private const int intervalSec = 30;
        public required string Exchange { get; set; }
        public required int PairCount { get; set; } = 1;
        public required int BatchSize { get; set; } = 1;
        public int BatchCount { get { return PairCount / BatchSize; } }
        public int DelaySecond { get { return intervalSec / BatchCount; } }
        public List<int> Pairs { get { return Enumerable.Range(1, PairCount).ToList(); } }
        public ConsoleColor FontColor
        {
	 get { return (ConsoleColor)new Random().Next(1, 15); }
        }

    }

    public class TimerFactory
    {
        private readonly ConcurrentDictionary<string, NamedTimer> timers;
        public int TimerCount { get { return timers.Count; } }
        public TimerFactory()
        {
	 timers = new();
        }
        public NamedTimer GetNewTimer(string exchange, double intervalSec)
        {
	 if (timers.TryGetValue(exchange, out var timer))
	 {
	     return timer;
	 }
	 timer = intervalSec == 0
	     ? new NamedTimer(exchange, timers.Count)
	     : new NamedTimer(exchange, intervalSec * 1000, timers.Count);

	 timers.TryAdd(exchange, timer);

	 return timer;
        }

    }

    public class NamedTimer : System.Timers.Timer
    {
        public string Name { get; }
        public int Sequence { get; }
        public NamedTimer(string name)
        {
	 Name = name;
        }
        public NamedTimer(string name, int sequence) : this(name)
        {
	 Sequence = sequence;
        }
        public NamedTimer(string name, double interval, int sequence) : base(interval)
        {
	 Name = name;
	 Sequence = sequence;
        }
    }

}
