namespace Crypto.Binance;

public static class BinanceExtension
{
    public static void AddBinanceServices(this IServiceCollection services)
    {
        services.AddSingleton<TickerFaçade>();
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    }
}