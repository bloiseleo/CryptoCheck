using CryptoCheck.Domain;
using CryptoCheck.Domain.Formatters;
using CryptoCheck.Domain.PriceRetrievers;
using CryptoCheck.Infra;
using CryptoCheck.Infra.Repositories;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CryptoCheck
{
    public class Program
    {
        private static PriceRepository priceRepository;
        private static void initializeDatabase()
        {
            DatabaseConnection db = new DatabaseConnection("Data Source=database.db");
            MigrationRunner mr = new MigrationRunner(db);
            mr.MigrateAll();
            priceRepository = new PriceRepository(db);
        }
        public static void Main(string[] args)
        {
            initializeDatabase();
            string currentDirectory = Directory.GetCurrentDirectory();
            string chromeBinaryLocation = Path.Combine(currentDirectory, "chrome", "win64-114.0.5735.90", "chrome-win64", "chrome.exe");
            string chromeDriverBinary = Path.Combine(currentDirectory, "chromedriver_win32", "chromedriver.exe");
            IWebDriver webDriver = new ChromeDriver(chromeDriverBinary, new ChromeOptions
            {
                BinaryLocation = chromeBinaryLocation,
            });
            IPriceRetriever priceRetriever = new CoinMarketCapPriceRetriever(
                new CoinMarketCapPriceFormatter()
            );
            CollectResult cr = priceRetriever.Collect(webDriver);
            priceRepository.Save(cr);
            IEnumerable<CollectResult> crs = priceRepository.FindAll();
        }
    }
}