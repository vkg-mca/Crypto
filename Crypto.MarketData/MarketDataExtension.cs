namespace Crypto.MarketData;

public static class MarketDataExtension
{
    public static void AddMarketDataServices(this IServiceCollection services)
    {
        services.AddHttpClient<IConnector, TextConnector>()
	  .SetHandlerLifetime(TimeSpan.FromMinutes(5))
	    .AddPolicyHandler(GetRetryPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
	 .HandleTransientHttpError()
	 .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
	 .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static CircuitBreakerPolicy GetCircuitBreakerPolicy()
    {
        return Policy
	  .Handle<HttpException>()
	  .CircuitBreaker(2, TimeSpan.FromMinutes(1));
    }
}