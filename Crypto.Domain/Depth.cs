namespace Crypto.Domain;

public class Depth
{
    public long lastUpdateId { get; set; }

    public double[] bids { get; set; }
    public double[] asks { get; set; }
}
