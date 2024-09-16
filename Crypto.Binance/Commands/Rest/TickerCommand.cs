namespace Crypto.Binance.Commands.Rest;

public class TickerCommand<T> : RestCommand<T> where T : class
{
    private readonly ILogger<TickerCommand<T>> _logger;
    public TickerCommand()
    {
        //https://api.binance.com/api/v3/ticker/bookTicker
    }

    public override void Execute()
    {
        Task<T> task = Connector.SendAsync<T>(CommandRequest);
        CommandResponse = new() { Response = task.Result }; ;    //TODO: Find better way to do this
    }

    public override async Task ExecuteAsync()
    {
        CommandResponse = new() { Response = await Connector.SendAsync<T>(CommandRequest) };    //TODO: Find better way to do this
    }

    //      public void Execute()
    //      {
    //Task<T> task = Connector.SendAsync<T>(CommandRequest);
    //var result = task.Result;
    ////CommandResponse = result;
    //GenericCommandResponse = new() { Response = task.Result };    //TODO: Find better way to do this
    //      }

    public async Task ExecuteAsync<TResponse>()
    {
        T result = await Connector.SendAsync<T>(CommandRequest);
        CommandResponse = new() { Response = result };    //TODO: Find better way to do this
    }

    public virtual CommandResponse<TResponse> ExecuteSync<TResponse>()
    {
        Task<CommandResponse<TResponse>> response = Connector.SendAsync<CommandResponse<TResponse>>(CommandRequest);
        return response.Result;
    }

}