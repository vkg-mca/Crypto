namespace Crypto.MarketData.Connectors;

public class Connector : IConnector
{
    public Connector(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string BaseUrl { get; set; }
    public KeyValue<string, string> ApiKey { get; set; }
    protected HttpClient _httpClient { get; set; }

    public virtual Task<T> SendAsync<T>(CommandRequest request)
    {
        throw new NotImplementedException();
    }

    protected virtual async Task<HttpResponseMessage> ExecuteAsyncWithRetry(HttpRequestMessage request, CancellationToken ct)
    {
        var timeoutPolicy = Policy.TimeoutAsync(30, TimeoutStrategy.Optimistic);
        HttpResponseMessage response = await timeoutPolicy
	      .ExecuteAsync(async ct => await _httpClient.SendAsync(request, ct), // Execute a delegate which responds to a CancellationToken input parameter.
	        CancellationToken.None // In this case, CancellationToken.None is passed into the execution, indicating you have no independent cancellation control you wish to add to the cancellation provided by TimeoutPolicy.  Your own independent CancellationToken can also be passed - see wiki for examples.
	        );

        return response;
        
    }
    protected virtual async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage request, CancellationToken ct)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request,ct).ConfigureAwait(false) ;
        return response;
    }

}
