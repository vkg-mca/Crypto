using ExchangeCrawler;
using ExchangeCrawler.ExchangeImplementation;

namespace PnlTickerDispatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
	 //ExchangeCrawlerService exchCrawlerSvc = new ExchangeCrawlerService();
	 //exchCrawlerSvc.Start();

	 //TickerProvider tickerProvider = new ();
	 //tickerProvider.Start();

	 ExchangeCrawler.Actual.TickerService svc = new();
	 svc.Start();

	 Console.ReadLine();
        }
    }
}