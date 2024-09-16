namespace Crypto.Console;

public static class CryptoConsole
{
    public static void WriteTitle(string title, Justify alignment = Justify.Left)
    {
        AnsiConsole
	 .Write(new FigletText(title)
	 .Justify(alignment)
	 .Color(Color.GreenYellow));
    }
    
    public static void WriteError(string title)
    {
        AnsiConsole.Foreground = Color.Red;
        AnsiConsole.Decoration = Decoration.RapidBlink;
        AnsiConsole.Write(title);
    }

    public static void WriteInfo(string title)
    {
        AnsiConsole.Foreground = Color.Yellow;
        AnsiConsole.Decoration = Decoration.SlowBlink;
        AnsiConsole.Write(title);
    }

}
