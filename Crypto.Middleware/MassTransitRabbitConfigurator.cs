namespace Crypto.Middleware;

public static class MassTransitRabbitConfigurator
{
	public static IBusControl ConfigureBus(RabbitMqSetting rabbitMqSetting)
	{
		var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
		{
			CreateBus(rabbitMqSetting, cfg);
		});

		return bus;
	}

	public static IBusControl ConfigureConsumer(RabbitMqSetting rabbitMqSetting)
	{
		var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
		{
			CreateBus(rabbitMqSetting, cfg);

			//cfg.ReceiveEndpoint(host, "queuename", e =>
			//{
			//    e.Consumer<MyConsumer>();
			//});
		});

		return bus;
	}

	private static void CreateBus(RabbitMqSetting rabbitMqSetting, IRabbitMqBusFactoryConfigurator cfg)
	{
		cfg.Host(rabbitMqSetting.Host, host =>
		{
			host.Username(rabbitMqSetting.UserName);
			host.Password(rabbitMqSetting.Password);
		});
	}
}