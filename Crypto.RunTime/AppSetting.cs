namespace Crypto.RunTime;

public class AppSetting
{
    public AppStartupSetting AppStartupSetting { get; set; }
    public RabbitMqSetting RabbitMqSetting { get; set; }
    public ServiceHostSetting ServiceHostSetting { get; set; }
}

public struct HostedServiceName
{
    public const string CryptoProducerService=nameof(CryptoProducerService);
    public const string CryptoConsumerService = nameof(CryptoConsumerService);
}