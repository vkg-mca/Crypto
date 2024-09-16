namespace Crypto.MarketData.Exceptions;

public class ServerException : HttpException
{
    public ServerException() : base() { }

    public ServerException(string message) : base(message)
    {
        Message = message;
    }

    public ServerException(string message, Exception innerException) : base(message, innerException)
    {
        Message = message;
    }

    public new string Message { get; protected set; }
}
