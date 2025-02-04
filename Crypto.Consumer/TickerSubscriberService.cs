namespace Crypto.Consumer;

public class TickerSubscriberService : ServiceBase, IService, IConsumer<MessageBag<Ticker[]>>, IServiceEvent<MessageBag<Ticker[]>>
{
	private readonly Subscriber _subscriber;
	private readonly TickerFaçade _tickerFaçade;


	public event EventHandler<MessageBag<Ticker[]>> EventOccurred;


	public TickerSubscriberService(ILogger<IService> logger, TickerFaçade tickerFaçade, Subscriber subscriber, AppSetting appSetting, TickerConsumer tickerConsumer)
	 : base(appSetting, logger)
	{
		_tickerFaçade = tickerFaçade;
		_subscriber = subscriber;
		EventOccurred += tickerConsumer.NewTickerArrived;
	}

	public async Task Consume(ConsumeContext<MessageBag<Ticker[]>> context)
	{
		// _logger.LogInformation($"Received Ticker: MessageId={context.Message.MessageId}, Count=:{context.Message.Message.Count()}");
		if (EventOccurred != null)
		{
			_ = Task.Run(() => { EventOccurred(this, context.Message); });
		}
	}

	//  public async Task Consume(ConsumeContext<MessageBag<Tickers>> context)
	//  {
	//     // _logger.LogInformation($"Received Ticker: MessageId={context.Message.MessageId}, Count=:{context.Message.Message.Count()}");
	//      if (EventOccurred != null)
	//      {
	//_ = Task.Run(() => { EventOccurred(this, context.Message); });
	//      }
	//  }

	protected override Task Execute()
	{
		return Task.CompletedTask;
	}
}
