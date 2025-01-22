using CryptoCheck.Domain;
using CryptoCheck.Domain.PriceRetrievers;
using CryptoCheck.Infra.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace CryptoCheck.Collectors
{
    public class CoinMarketCapCollector(IPriceRetriever priceRetriever, PriceRepository priceRepository, IWebDriver webDriver, ILogger<CoinMarketCapCollector> logger) : BackgroundService
    {        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Collection started");
                CollectResult cr = priceRetriever.Collect(webDriver);
                logger.LogInformation($"Collected {cr}");
                priceRepository.Save(cr);
                logger.LogInformation("Persisted successfully");
                logger.LogInformation($"Sleeping for 60s");
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
