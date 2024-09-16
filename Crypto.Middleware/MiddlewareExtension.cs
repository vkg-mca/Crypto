namespace Crypto.Middleware;

public static class MiddlewareExtension
{
    public static void AddMiddlewareServices(this IServiceCollection services)
    {
        services.AddSingleton<AppSetting>();
        services.AddSingleton<Publisher>();
        services.AddSingleton<Subscriber>();
    }

    public static void AddRabbitMq(this ServiceCollection services, RabbitMqSetting rabbitMqSetting)
    {
        var bus = MassTransitRabbitConfigurator.ConfigureBus(rabbitMqSetting);
        services.AddSingleton(bus);
        Task.Run(bus.Start);
        var status = bus.WaitForHealthStatus(BusHealthStatus.Healthy, TimeSpan.FromSeconds(60)).Result;
        while (status != BusHealthStatus.Healthy)
        {
	 var health = bus.CheckHealth();
	 var result = bus.GetProbeResult();
        }
    }

   
}