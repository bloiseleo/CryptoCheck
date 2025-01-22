using CryptoCheck.Infra;
using CryptoCheck.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using CryptoCheck.Domain.PriceRetrievers;
using CryptoCheck.Domain.Formatters;
using CryptoCheck.Collectors;

namespace CryptoCheck
{
    public static class StartUp
    {
        public static HostApplicationBuilder UseDatabaseConnection(this HostApplicationBuilder host)
        {
            DatabaseConnection db = new DatabaseConnection("Data Source=database.db");
            new MigrationRunner(db).MigrateAll();
            host.Services.AddSingleton(config => db);
            host.Services.AddSingleton<PriceRepository>();
            return host;
        }
        public static HostApplicationBuilder UseWebDriver(this HostApplicationBuilder host)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string chromeBinaryLocation = Path.Combine(currentDirectory, "chrome", "win64-114.0.5735.90", "chrome-win64", "chrome.exe");
            string chromeDriverBinary = Path.Combine(currentDirectory, "chromedriver_win32", "chromedriver.exe");
            IWebDriver webDriver = new ChromeDriver(chromeDriverBinary, new ChromeOptions
            {
                BinaryLocation = chromeBinaryLocation,
            });
            host.Services.AddSingleton(webDriver);
            return host;
        }
        public static HostApplicationBuilder UsePriceRetrievers(this HostApplicationBuilder host)
        {
            host.Services.AddSingleton<IPriceFormatter, CoinMarketCapPriceFormatter>();
            host.Services.AddSingleton<IPriceRetriever, CoinMarketCapPriceRetriever>();
            return host;
        }
        public static HostApplicationBuilder UseCollectorsBackgroundService(this HostApplicationBuilder host)
        {
            host.Services.AddHostedService<CoinMarketCapCollector>();
            return host;
        }
    }
}
