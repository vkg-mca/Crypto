namespace Crypto.Domain;

public class Ticker
{
    public string symbol { get; set; }

    [JsonConverter(typeof(DecimalConverter))]
    public decimal bidPrice { get; set; }

    [JsonConverter(typeof(DecimalConverter))]
    public decimal bidQty { get; set; }

    [JsonConverter(typeof(DecimalConverter))]
    public decimal askPrice { get; set; }

    [JsonConverter(typeof(DecimalConverter))]
    public decimal askQty { get; set; }

    public Ticker Clone()
        => new Ticker { askPrice = askPrice, askQty = askQty, bidPrice = bidPrice, bidQty = bidQty, symbol = symbol };
}

public class Tickers : List<Ticker>
{
    public Tickers() : base() { }
    public Tickers(int capacity) : base(capacity) { }
    public Tickers(Ticker[] tickers) : base(tickers) { }
    public Tickers(Ticker ticker) : this(new Ticker[1] { ticker }) { }

}