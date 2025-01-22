using Microsoft.Extensions.Hosting;

namespace CryptoCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.UseDatabaseConnection();
            builder.UseWebDriver();
            builder.UsePriceRetrievers();
            builder.UseCollectorsBackgroundService();
            IHost app = builder.Build();
            app.Run();
        }
    }
}