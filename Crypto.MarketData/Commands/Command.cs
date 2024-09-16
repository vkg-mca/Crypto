namespace Crypto.MarketData.Commands;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class Command<T> : ICommand<T> where T : class
{
    public CommandRequest CommandRequest { get; set; }
    //public CommandResponse CommandResponse { get; set; }

    public IConnector Connector { get; set; }
    public CommandResponse<T> CommandResponse { get; set; }

    public virtual void Execute() //where T : CommandResponse
    {
        Task<T> response = Connector.SendAsync<T>(CommandRequest);
        CommandResponse = new CommandResponse<T> { Response = response.Result };
        //CommandResponse = response.Result;
    }

    //      public virtual void Execute()
    //      {
    //Task<CommandResponse> response = Connector.SendAsync<CommandResponse>(CommandRequest);
    ////CommandResponse = response.Result;
    //      }

    public virtual CommandResponse<T> ExecuteSync()
    {
        Task<T> response = Connector.SendAsync<T>(CommandRequest);
        //CommandResponse = new CommandResponse<T>() { Response = response.Result };
        CommandResponse = new CommandResponse<T> { Response = response.Result };
        return CommandResponse;

    }

    public async virtual Task ExecuteAsync()
    {
        var response = await Connector.SendAsync<T>(CommandRequest);
        CommandResponse = new CommandResponse<T> { Response = response };
    }

    //      public async virtual Task ExecuteAsync<TResponse>() where TResponse : CommandResponse
    //      {
    //TResponse response = await Connector.SendAsync<TResponse>(CommandRequest);
    ////CommandResponse = response;
    //      }

    public string GetDebuggerDisplay()
    {
        return ToString();
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
