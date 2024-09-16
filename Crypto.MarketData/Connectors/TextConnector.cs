using System.Text.Unicode;

namespace Crypto.MarketData.Connectors;

public class TextConnector : Connector
{
    public TextConnector(HttpClient httpClient) : base(httpClient)
    {

    }

    public async override Task<TResponse> SendAsync<TResponse>(CommandRequest httpRequest)
    {
        using var request = new HttpRequestMessage(httpRequest.Method, BaseUrl + httpRequest.EndPoint);
        if (httpRequest.Content is not null)
        {
	 request.Content = new StringContent(JsonConvert.SerializeObject(httpRequest.Content), Encoding.UTF8, "application/json");
        }

        if (ApiKey != default && ApiKey.Key != default && ApiKey.Value != default)
        {
	 request.Headers.Add(ApiKey.Key, ApiKey.Value);
        }
  
        CancellationTokenSource cts = new();
        HttpResponseMessage response = await ExecuteAsync(request, cts.Token);

        if (response.IsSuccessStatusCode)
        {
	 using HttpContent responseContent = response.Content;
	 string jsonString = await responseContent.ReadAsStringAsync();

	 if (typeof(TResponse) == typeof(string))
	 {
	     return (TResponse)(object)jsonString;
	 }
	 else
	 {
	     try
	     {
	         TResponse data = JsonConvert.DeserializeObject<TResponse>(jsonString);
	         
	         return data;
	     }
	     catch (JsonReaderException ex)
	     {
	         var clientException = new ClientException($"Failed to map server response from '${httpRequest.EndPoint}' to given type", -1, ex)
	         {
		  StatusCode = (int)response.StatusCode,
		  Headers = response.Headers.ToDictionary(a => a.Key, a => a.Value)
	         };

	         throw clientException;
	     }
	 }
        }
        else
        {
	 using HttpContent responseContent = response.Content;
	 HttpException httpException = null;
	 string contentString = await responseContent.ReadAsStringAsync();
	 int statusCode = (int)response.StatusCode;
	 if (statusCode is >= 400 and < 500)
	 {
	     if (string.IsNullOrWhiteSpace(contentString))
	     {
	         httpException = new ClientException("Unsuccessful response with no content", -1);
	     }
	     else
	     {
	         try
	         {
		  httpException = JsonConvert.DeserializeObject<ClientException>(contentString);
	         }
	         catch (JsonReaderException ex)
	         {
		  httpException = new ClientException(contentString, -1, ex);
	         }
	     }
	 }
	 else
	 {
	     httpException = new ServerException(contentString);
	 }

	 httpException.StatusCode = statusCode;
	 httpException.Headers = response.Headers.ToDictionary(a => a.Key, a => a.Value);

	 throw httpException;
        }
    }
}
