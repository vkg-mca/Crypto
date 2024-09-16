namespace Crypto.Producer;

public static class ServiceExtension
{
    public static IService AddTickerProducerService(this ServiceCollection services,IServiceProvider serviceProvider, AppSetting appSettings)
    {
        var publisher = serviceProvider.GetService<Publisher>();
        var tickerFaçade = serviceProvider.GetService<TickerFaçade>();
        var logger = serviceProvider.GetRequiredService<ILogger<IService>>();
        var tickerService = new TickerPublisherService(logger, tickerFaçade, publisher, appSettings);
        services.AddSingleton(tickerService);
        return tickerService;
    }
}
