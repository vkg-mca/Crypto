namespace Crypto.RunTime;

public class AppStartupSetting
{
    public Dictionary<string, List<string>> EnabledExchanges { get; set; }

    //[Required]
    public required int IterationFrequencySec { get; set; }
}