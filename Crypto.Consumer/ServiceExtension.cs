namespace Crypto.Consumer;

public static class CryptoServiceExtension
{
	public static IService AddTickerSubscriberService(this IServiceCollection services, IServiceProvider serviceProvider, AppSetting appSettings)
	{
		var subscriber = serviceProvider.GetService<Subscriber>();
		var tickerFaçade = serviceProvider.GetService<TickerFaçade>();
		var logger = serviceProvider.GetRequiredService<ILogger<IService>>();
		var consumer = serviceProvider.GetRequiredService<TickerConsumer>();
		var tickerService = new TickerSubscriberService(logger, tickerFaçade, subscriber, appSettings, consumer);
		services.AddSingleton(tickerService);
		return tickerService;
	}

	public static void AddTickerConsumerService(this IServiceCollection services, AppSetting appSetting)
	{
		services.AddSingleton(new TickerConsumer(appSetting.AppStartupSetting));
	}

	public static void AddMassTransitTickerSubscriberService(this ServiceCollection services, RabbitMqSetting rabbitMqSetting)
	{
		services.AddMassTransit(x =>
		{
			x.AddConsumer<TickerSubscriberService>();

			//x.UsingRabbitMq((context, cfg) =>
			//{
			//    cfg.ConfigureEndpoints(context);
			//});

			x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
			{
				cfg.Host(rabbitMqSetting.Host, h =>
				{
					h.Username(rabbitMqSetting.UserName);
					h.Password(rabbitMqSetting.Password);
				});

				//cfg.ClearMessageDeserializers();

				//var deserializer = JsonSerializer.CreateDefault();
				//var jsonContent = new ContentType("application/json");
				//cfg.AddMessageDeserializer(jsonContent, () => new RawJsonMessageDeserializer(jsonContent, deserializer));
				//var mstJsonContent = new ContentType("application/vnd.masstransit+json");
				//cfg.AddMessageDeserializer(mstJsonContent, () => new RawJsonMessageDeserializer(mstJsonContent, deserializer));

				//cfg.SetLoggerFactory(provider.GetService<ILoggerFactory>());

				cfg.ReceiveEndpoint("masstransit.mqttconsumer", e =>
				{
					e.Bind("masstransit.mqtt", x =>
					{
						x.ExchangeType = ExchangeType.Topic;
						x.RoutingKey = "#";// route all exchange messages to our queue
					});

					e.ConfigureConsumer<TickerSubscriberService>(provider);
					//EndpointConvention.Map<MessageBag>(e.InputAddress);
				});
			}));
		});

	}
}
