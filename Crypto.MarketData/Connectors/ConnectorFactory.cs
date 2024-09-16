namespace Crypto.MarketData.Connectors;

public class ConnectorFactory
{
    public static IConnector GetInstance(EnvVar.ConnectorProtocolType protocolType, EnvVar.ConnectorResponseType responseType, HttpClient httpClient)
    {
        return (protocolType, responseType) switch
        {
	 (EnvVar.ConnectorProtocolType.Rest, EnvVar.ConnectorResponseType.Text)
	     => new TextConnector(httpClient),

	 (EnvVar.ConnectorProtocolType.Rest, EnvVar.ConnectorResponseType.Byte)
	     => new ByteConnector(httpClient),
        };
    }
}
