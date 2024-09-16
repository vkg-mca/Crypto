namespace Crypto.Utility;

public class SyncTimers
{
    private readonly Dictionary<string, SyncTimer> _syncTimers;

    public SyncTimers()
    {
        _syncTimers = new();
    }

    public bool Start(string name, int intervalSec, Action<object, ElapsedEventArgs> callback)
    {
        var timer = new SyncTimer(intervalSec, callback, name);

        return _syncTimers.ContainsKey(name)
	 ? throw new Exception($"Timer with name {name} already exists")
	 : !_syncTimers.TryAdd(name, timer) ? throw new Exception($"Timer {name} cannot start") : timer.Start();
    }

    public bool Stop(string name)
    {
        if (_syncTimers.ContainsKey(name))
	 return false;

        var timer = _syncTimers[name];
        _syncTimers.Remove(name);

        return timer.Stop();
    }
}