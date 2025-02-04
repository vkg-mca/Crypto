namespace Crypto.MarketData.Connectors;

public class ByteConnector : Connector
{
	protected readonly ArrayPool<byte> arrayPool = ArrayPool<byte>.Shared;
	private readonly bool useStream = true;

	public ByteConnector(HttpClient httpClient) : base(httpClient)
	{

	}

	public async override Task<TResponse> SendAsync<TResponse>(CommandRequest httpRequest)
	{
		return useStream
			? await SendAsyncUsingStream<TResponse>(httpRequest)
			: await SendAsyncUsingBytes<TResponse>(httpRequest);
	}

	private async Task<TResponse> SendAsyncUsingStream<TResponse>(CommandRequest httpRequest)
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
			try
			{
				TResponse result = ByteSerializer.Deserialize<TResponse>(responseContent.ReadAsStream());
				return result;
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

		else
		{
			using HttpContent responseContent = response.Content;
			HttpException httpException = null;
			string contentString = await responseContent.ReadAsStringAsync();
			var contentBytes = responseContent.ReadAsStream();
			int statusCode = (int)response.StatusCode;
			if (statusCode is >= 400 and < 500)
			{
				if (contentBytes == null || contentBytes.Length == 0)
				{
					httpException = new ClientException("Unsuccessful response with no content", -1);
				}
				else
				{
					try
					{
						httpException = ByteSerializer.Deserialize<HttpException>(contentBytes);
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

	private async Task<TResponse> SendAsyncUsingBytes<TResponse>(CommandRequest httpRequest)
	{
		int contentBytesCount = 0;
		// ReadOnlySpan<byte> contentBytesMemory;
		using var request = new HttpRequestMessage(httpRequest.Method, BaseUrl + httpRequest.EndPoint);
		if (httpRequest.Content is not null)
		{
			request.Content = new StringContent(JsonConvert.SerializeObject(httpRequest.Content), Encoding.UTF8, "application/json");
		}

		if (ApiKey != default && ApiKey.Key != default && ApiKey.Value != default)
		{
			request.Headers.Add(ApiKey.Key, ApiKey.Value);
		}

		HttpResponseMessage response = await _httpClient.SendAsync(request);

		if (response.IsSuccessStatusCode)
		{
			using HttpContent responseContent = response.Content;
			var contentBytes = responseContent.ReadAsStream().ToRentedArray(arrayPool, out contentBytesCount);
			ReadOnlyMemory<byte> contentBytesMemory = new(contentBytes, 0, contentBytesCount);

			try
			{
				//TResponse data = TextSerializer.Deserialize<TResponse>(contentBytesMemory.Span , contentBytesCount);
				TResponse result = ByteSerializer.Deserialize<TResponse>(contentBytesMemory.Span);
				return result;
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
			finally
			{
				if (contentBytes != null)
				{
					arrayPool.Return(contentBytes);
				}
			}
		}

		else
		{
			using HttpContent responseContent = response.Content;
			HttpException httpException = null;
			string contentString = await responseContent.ReadAsStringAsync();
			var contentBytes = responseContent.ReadAsStream().ToRentedArray(arrayPool, out contentBytesCount);
			int statusCode = (int)response.StatusCode;
			if (statusCode is >= 400 and < 500)
			{
				if (contentBytes == null || contentBytes.Length == 0)
				{
					httpException = new ClientException("Unsuccessful response with no content", -1);
				}
				else
				{
					try
					{
						ReadOnlyMemory<byte> contentBytesMemory = new(contentBytes, 0, contentBytesCount);
						httpException = ByteSerializer.Deserialize<HttpException>(contentBytesMemory.Span);
					}
					catch (JsonReaderException ex)
					{
						httpException = new ClientException(contentString, -1, ex);
					}
					finally
					{
						if (contentBytes != null)
						{
							arrayPool.Return(contentBytes);
						}
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
