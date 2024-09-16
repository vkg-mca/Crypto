namespace Crypto.MarketData.Exceptions;

public class ClientException : HttpException
{
    public ClientException() : base() { }

    public ClientException(string message, int code) : base(message)
    {
        (Message, Code) = (message, code);
    }

    public ClientException(string message, int code, Exception innerException) : base(message, innerException)
    {
        (Message, Code) = (message, code);
    }

    [Newtonsoft.Json.JsonProperty("code")]
    public int Code { get; set; }

    [Newtonsoft.Json.JsonProperty("msg")]
    public new string Message { get; protected set; }
}
