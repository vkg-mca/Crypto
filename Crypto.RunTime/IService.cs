namespace Crypto.RunTime;

public interface IService
{
    //public string ServiceName { get => string.Empty; }
    //public string ServiceInstanceName { get => string.Empty; }
    //public string ServiceDisplayName { get => string.Empty; }
    //public string ServiceDescription { get => string.Empty; }
    //public string InstanceName { get; protected set; }
    public void Start();
    public void Stop();
    


}

public interface IServiceEvent<TEventArgs>
{
    event EventHandler<TEventArgs> EventOccurred;
}

//public class ServiceEventArgs <T>
//{ 

//}
