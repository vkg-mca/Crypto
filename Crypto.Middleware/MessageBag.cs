namespace Crypto.Middleware;

public class MessageBag
{
    public Guid MessageId { get; set; } = Guid.NewGuid(); 
}

public class MessageBag<T> : MessageBag
{
    public MessageBag(T message)
    {
        Message = message;
    }

    public T Message { get; set; }

}

public class TickerBag : MessageBag<Ticker[]>
{
    Tickers _tickers;
    public TickerBag(Tickers tickers) : base(tickers.ToArray())
    {
        _tickers = tickers;
    }

    public TickerBag(Ticker[] tickers) : base(tickers)
    {
        _tickers = new Tickers(tickers.Count()  );
        _tickers.AddRange(tickers);
    }

    public TickerBag Clone()
    {
        if (_tickers == null || _tickers.Count == 0)
	 return null;

        Tickers tickers = new(_tickers.Count); ;
        foreach (var ticker in _tickers)
        {
	 tickers.Add(ticker.Clone());  
        }
        return new TickerBag(tickers);
        
    }

    
}
