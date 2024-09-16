namespace Crypto.RunTime;

public class ServiceHostSetting
{
    public ServiceHostInstance[] ServiceHost { get; set; }
}

public class ServiceHostInstance
{
    public string ServiceInstanceName { get; set; } 

    public string ServiceName { get; set; }

    public string ServiceDisplayName { get; set; }

    public string ServiceDescription { get; set; }
}

