namespace Crypto.Logging;

public static class LoggingExtension
{
    public static void AddNLog(this IServiceCollection services)
        => AddNLog(services, LogLevel.Trace, string.Empty);

    public static void AddNLog(this IServiceCollection services, LogLevel logLevel)
        => AddNLog(services,logLevel, string.Empty);

    public static void AddNLog(this IServiceCollection services, LogLevel logLevel, string logConfigFile)
    {
        services.AddLogging(loggingBuilder =>
        {
	 // configure Logging with NLog
	 loggingBuilder.ClearProviders();
	 loggingBuilder.SetMinimumLevel(logLevel);
	 loggingBuilder.AddNLog(string.IsNullOrWhiteSpace(logConfigFile)? "nlog.config" : logConfigFile);
        });
    }

    public static IServiceCollection AddDefaultLogging(this IServiceCollection services)
    {
        services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
        services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        return services;
    }
}