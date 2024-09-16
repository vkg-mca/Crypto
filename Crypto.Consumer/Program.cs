namespace Crypto.Consumer;

internal class Program
{
    private static ServiceHost _tickerServiceHost;

    private static void Main(string[] args)
    {
        PreProcess();
        Process();
        PostProcess();
    }

    private static void PreProcess()
    {
        CryptoConsole.WriteTitle("Crypto Service", Justify.Center);
        CryptoConsole.WriteTitle("Consumer", Justify.Center);
        CryptoConsole.WriteTitle("------------", Justify.Center);
        EnvironmentSettings.SetCurrentDirectory();
        EnvironmentSettings.SetApplicationTitle("Crypto Service");
    }

    private static void Process()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddRuntimeServices(out var appSettings);

        //var services = new ServiceCollection();

        ServiceProvider serviceProvider = default;
        var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

        builder.ConfigureServices((hostContext, services) =>
        {
	 services.AddNLog();
	 services.AddMarketDataServices();
	 services.AddMiddlewareServices();
	 services.AddBinanceServices();
	 services.AddDefaultLogging();
	 //services.ConfigureConsumer(appSettings.RabbitMqSetting);
	 //services.AddMassTransitConsumerService(appSettings.RabbitMqSetting);
	 services.AddTickerConsumerService(appSettings);
	 serviceProvider = services.BuildServiceProvider();

	 var tickerService = services.AddTickerSubscriberService(serviceProvider, appSettings);

	 //var logger = serviceProvider.GetRequiredService<ILogger<ServiceHost>>();
	 //_tickerServiceHost = new ServiceHost(logger);
	 //_tickerServiceHost.HostServiceAsWindowsService(tickerService);
	 services.AddMassTransit(busConfigurator =>
	 {
	     var entryAssembly = Assembly.GetExecutingAssembly();

	     busConfigurator.AddConsumers(entryAssembly);
	     busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
	     {
	         busFactoryConfigurator.Host("localhost", "/", h => { });

	         busFactoryConfigurator.ConfigureEndpoints(context);
	     });
	 });
        });
        var app = builder.Build();

        app.Run();

        //var logger = serviceProvider.GetRequiredService<ILogger<ServiceHost>>();
        //_tickerServiceHost = new ServiceHost(logger, appSettings.ServiceHostSetting.ServiceHost.Where(x => x.ServiceName.Equals(HostedServiceName.CryptoConsumerService, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
        //_tickerServiceHost.HostAsConsoleApp(builder); 
       
    }

    private static void Process1()
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddRuntimeServices(out var appSettings);

        var services = new ServiceCollection();
        services.AddNLog();
        services.AddMarketDataServices();
        services.AddMiddlewareServices();
        services.AddBinanceServices();
        services.AddDefaultLogging();
        services.AddRabbitMq(appSettings.RabbitMqSetting);
        services.ConfigureConsumer(appSettings.RabbitMqSetting);
        //services.AddMassTransitConsumerService(appSettings.RabbitMqSetting);
        var serviceProvider = services.BuildServiceProvider();
        var tickerService = services.AddTickerSubscriberService(serviceProvider, appSettings);

        var logger = serviceProvider.GetRequiredService<ILogger<ServiceHost>>();
        _tickerServiceHost = new ServiceHost(logger, appSettings.ServiceHostSetting.ServiceHost.Where(x => x.ServiceName.Equals(HostedServiceName.CryptoConsumerService, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
        _tickerServiceHost.HostAsWindowsService(tickerService);
    }

    private static void PostProcess()
    {
        EnvironmentSettings.Wait();
    }
}

public static class EEConsumerExtension1
{
    public static void ConfigureConsumer(this IServiceCollection services, RabbitMqSetting rabbitMqSettings)
    {
        services.AddMassTransit(mt =>
	 mt.UsingRabbitMq((cntxt, cfg) =>
	 {
	     var cc = cntxt;
	     var css = cfg;
	     cfg.Host(rabbitMqSettings.Host, "/", c =>
	     {
	         var cc = c;
	         c.Username(rabbitMqSettings.UserName);
	         c.Password(rabbitMqSettings.Password);
	     });

	     //cfg.ReceiveEndpoint("queue01", ep => ep.Consumer(() => new Consumer<TickerConsumerService>()));
	     cfg.ReceiveEndpoint("queue01", ep => ep.ConfigureConsumer<TickerSubscriberService>(cntxt));

	     cfg.ConfigureEndpoints(cntxt);
	     mt.AddConsumer<TickerSubscriberService>();
	     cfg.ReceiveEndpoint("queue01", (c) =>
	     {
	         c.Bind("Crypto.Middleware:TickerMessageBag", x =>
	         {
		  x.ExchangeType = ExchangeType.Fanout;

		  // route all exchange messages to our queue
		  x.RoutingKey = "#";
	         });

	         c.ConfigureConsumer<TickerSubscriberService>(cntxt);
	     });
	 }));
    }
}