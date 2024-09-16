namespace Crypto.RunTime;

public class RabbitMqSetting
{
    public string Host { get; set; } = "localhsot";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public ushort Port { get; set; } = 5672;
}
