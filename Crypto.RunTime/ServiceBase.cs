namespace Crypto.RunTime;

public abstract class ServiceBase: IService
{
    protected readonly SyncTimer _serviceTimer;
    protected readonly ILogger<IService> _logger;

    public ServiceBase(AppSetting appSetting, ILogger<IService> logger)
    {
        _logger = logger;
        _serviceTimer = new(appSetting.AppStartupSetting.IterationFrequencySec, OnTimerCallback, nameof(_serviceTimer));
    }

    protected virtual async void OnTimerCallback(object sender, ElapsedEventArgs e)
        => await Execute();

    protected abstract Task Execute();

    public virtual void Start()
    {
        _serviceTimer.Start();
    }

    public virtual void Stop()
    {
        _serviceTimer.Stop();
    }
}
