namespace Crypto.RunTime;

public class ServiceHost
{
	private readonly ILogger<ServiceHost> _logger;
	private readonly ServiceHostInstance _serviceHost;

	public ServiceHost(ILogger<ServiceHost> logger, ServiceHostInstance serviceHost)
	{
		_logger = logger;
		_serviceHost = serviceHost;
	}

	public void HostAsWindowsService(IService service)
	{
		try
		{
			var exitCode = HostFactory.Run(hostConfig =>
			{
				hostConfig.Service<IService>(serviceConfig =>
				{
					serviceConfig.BeforeStartingService(ctx => ctx.RequestAdditionalTime(TimeSpan.FromSeconds(1)));
					serviceConfig.BeforeStoppingService(ctx => ctx.RequestAdditionalTime(TimeSpan.FromSeconds(1)));
					serviceConfig.ConstructUsing(svc => service);
					serviceConfig.WhenStarted(svc => svc.Start());
					serviceConfig.WhenStopped(svc => svc.Stop());
					serviceConfig.WhenShutdown(svc => svc.Stop());
				});
				//hostConfig.UseNLog();
				hostConfig.EnableShutdown();
				hostConfig.RunAsLocalSystem();
				hostConfig.StartAutomatically();
				hostConfig.EnableServiceRecovery(recoveryConfig => { recoveryConfig.RestartService(1); });
				hostConfig.SetInstanceName(_serviceHost.ServiceInstanceName);
				hostConfig.SetServiceName(_serviceHost.ServiceName);
				hostConfig.SetDisplayName(_serviceHost.ServiceDisplayName);
				hostConfig.SetDescription(_serviceHost.ServiceDescription);
			});
			int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
			Environment.ExitCode = exitCodeValue;
			_logger.LogInformation($"Hosted {_serviceHost.ServiceName} successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Failed to host {_serviceHost.ServiceName}");
			if (Debugger.IsAttached)
			{
				Debugger.Break();
				Debugger.Log(0, "Rabbit", ex.Message);
			}
			Environment.Exit(-1);
		}
	}
	public void HostAsConsoleApp(IHostBuilder hostBuilder)
	{
		var app = hostBuilder.Build();

		app.Run();
	}
}
