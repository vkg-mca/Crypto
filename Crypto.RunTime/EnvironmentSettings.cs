namespace Crypto.RunTime;

public class EnvironmentSettings
{
    public static bool AppHostedAsConsole
	 => HostingEnvironment == HostingEnvironment.CONSOLE;

    public static bool AppHostedAsService
	 => HostingEnvironment == HostingEnvironment.SERVICE;

    public static HostingEnvironment HostingEnvironment
	 => Environment.UserInteractive
	 ? HostingEnvironment.CONSOLE
	 : HostingEnvironment.SERVICE;

    public static void SetCurrentDirectory()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
    }

    public static void SetApplicationTitle(string title)
    {
        Console.Title = title;
    }

    public static bool Wait()
    {
        if (AppHostedAsConsole)
        {
	 Console.ReadLine();
        }
        return false;
    }

}

public enum HostingEnvironment
{
    CONSOLE,
    SERVICE
}