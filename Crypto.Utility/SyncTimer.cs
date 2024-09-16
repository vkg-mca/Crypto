namespace Crypto.Utility;

public class SyncTimer// : System.Timers.Timer
{
    private System.Timers.Timer _timer;
    private readonly int _intervalSec;
    private readonly ElapsedEventHandler _elapsedEventHandler;
    private readonly Action<object, ElapsedEventArgs> _callback;

    public string Name { get; }

    public SyncTimer(int intervalSec, Action<object, ElapsedEventArgs> callback, string name)
    {
        Name = name;
        _callback = callback;
        _intervalSec = intervalSec;
    }

    public bool Start()
    {
        if (_intervalSec <= 0)
	 return false;

        _timer ??= new System.Timers.Timer(_intervalSec * 1000);

        if (_elapsedEventHandler != null)
	 _timer.Elapsed += _elapsedEventHandler;

        if (_callback != null)
	 _timer.Elapsed += new ElapsedEventHandler(_callback);

        _timer.Enabled = true;

        _timer.Start();

        return true;
    }

    public bool Stop()
    {
        if (_timer == default)
	 return false;

        if (_elapsedEventHandler != null)
	 _timer.Elapsed -= _elapsedEventHandler;

        if (_callback != null)
	 _timer.Elapsed -= new ElapsedEventHandler(_callback);

        _timer.Enabled = false;

        _timer.Stop();

        return true;
    }
}