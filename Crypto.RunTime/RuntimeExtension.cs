namespace Crypto.Runtime;

public static class RuntimeExtension
{
    public static IConfiguration AddRuntimeServices(this IConfigurationBuilder configBuilder, out AppSetting appSetting)
    {
        configBuilder
	 .SetBasePath(Directory.GetCurrentDirectory())
	 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	 .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
	 .AddEnvironmentVariables();
	 
        var config = configBuilder.Build();
        
        appSetting = config.GetSection("AppSetting").Get<AppSetting>();
        return config;
    }

}


//public class Startup
//{
//    public void BuildApplicationSettings()
//    {
//        var configSettings = BuildAppSetting();
//        var config = configSettings.Build();
//        AppSetting = config.GetSection("AppSetting").Get<AppSetting>();
//    }

//    private static IConfigurationBuilder BuildAppSetting()
//    {
//        var builder = new ConfigurationBuilder()
//	      .SetBasePath(Directory.GetCurrentDirectory())
//	      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//	      .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
//	      .AddEnvironmentVariables();

//        return builder;
//    }

//    public AppSetting AppSetting { get; private set; }
//}
