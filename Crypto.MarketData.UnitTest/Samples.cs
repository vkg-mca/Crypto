using Microsoft.Extensions.Hosting;
namespace Crypto.MarketData.UnitTest;

internal class Samples
{
    private void HostBuilderSample()
    {
        var host = new HostBuilder()
         .ConfigureHostConfiguration(config =>
         {
             // Configure the host environment here
         })
         .ConfigureAppConfiguration((hostContext, config) =>
         {
             // Configure the application configuration here
         })
         .ConfigureServices((hostContext, services) =>
         {
             // Register services with the DI container here
         })
         .Build();

        // Start the host
        host.Run();
    }

}
