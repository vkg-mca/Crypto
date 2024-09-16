namespace Crypto.MarketData.Environment;

public class EnvVar
{
    public enum ConnectorResponseType
    {
        Text,
        Byte
    }

    public enum ConnectorProtocolType
    {
        Rest,
        WebSocket
    }
}
