namespace Crypto.Producer;

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
        CryptoConsole.WriteTitle("Producer", Justify.Center);
        CryptoConsole.WriteTitle("------------", Justify.Center);
        EnvironmentSettings.SetCurrentDirectory();
        EnvironmentSettings.SetApplicationTitle("Crypto Service");
    }

    private static void Process()
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

        var serviceProvider = services.BuildServiceProvider();
        var tickerService = services.AddTickerProducerService(serviceProvider, appSettings);

        var logger = serviceProvider.GetRequiredService<ILogger<ServiceHost>>();
        var producerService = appSettings.ServiceHostSetting.ServiceHost.Where(x => x.ServiceName.Equals(HostedServiceName.CryptoProducerService, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        _tickerServiceHost = new ServiceHost(logger, producerService);
        _tickerServiceHost.HostAsWindowsService(tickerService);
    }

    private static void PostProcess()
    {
        EnvironmentSettings.Wait();
    }
}