namespace Crypto.Middleware;

public class Publisher
{
	private readonly IBusControl _bus;
	private readonly ILogger<Publisher> _logger;

	public Publisher(IBusControl bus, ILogger<Publisher> logger)
	{
		_bus = bus;
		_logger = logger;
	}

	public async Task<bool> PublishAsync<T>(MessageBag<T> message, CancellationToken ct)
	{
		try
		{
			await _bus.Publish(message, ct).ConfigureAwait(false);
			_logger.LogInformation($"Published new message with id {message.MessageId}");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Failed to publish message with id {message.MessageId}");
			if (Debugger.IsAttached)
			{
				Debugger.Break();
				Debugger.Log(0, "Rabbit", ex.Message);
			}
		}

		return true;
	}

	public async Task<bool> PublishAsync<T>(T message, CancellationToken ct)
		=> await PublishAsync(new MessageBag<T>(message), ct)
		.ConfigureAwait(false);
}