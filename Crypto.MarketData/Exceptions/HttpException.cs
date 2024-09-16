namespace Crypto.MarketData.Exceptions;

public class HttpException : Exception
{
    public HttpException() : base() { }

    public HttpException(string message) : base(message) { }

    public HttpException(string message, Exception innerException) : base(message, innerException) { }

    public int StatusCode { get; set; }

    public Dictionary<string, IEnumerable<string>> Headers { get; set; }
}
