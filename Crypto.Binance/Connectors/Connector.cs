namespace Crypto.Binance.Connectors;

public class Connector : TextConnector
{
   
    public Connector(HttpClient httpClient) : base(httpClient)
    {
        //Swagger: https://binance.github.io/binance-api-swagger/
        BaseUrl = "https://api.binance.com";
        ApiKey = new KeyValue<string, string>("X-MBX-APIKEY", default);
    }
}
