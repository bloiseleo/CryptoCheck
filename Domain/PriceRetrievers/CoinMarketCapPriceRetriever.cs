using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCheck.Domain.Exceptions;
using CryptoCheck.Domain.Formatters;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CryptoCheck.Domain.PriceRetrievers
{
    public class CoinMarketCapPriceRetriever(IPriceFormatter priceFormatter) : IPriceRetriever
    {
        private string _url = "https://coinmarketcap.com/currencies/bitcoin/";
        private By _bitcoinSelector = By.CssSelector("#section-coin-overview > div.sc-65e7f566-0.czwNaM.flexStart.alignBaseline > span");
        private void awaitUntilElementOnScreen(IWebDriver webDriver)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(d => webDriver.FindElements(_bitcoinSelector).Count > 0);
        }
        public CollectResult Collect(IWebDriver webDriver)
        {
            webDriver.Navigate().GoToUrl(_url);
            awaitUntilElementOnScreen(webDriver);
            IWebElement bitcoinSpan = webDriver.FindElement(_bitcoinSelector);
            var bitcoinValueUnformatted = bitcoinSpan.Text;
            Console.WriteLine(bitcoinSpan.Text);
            if (bitcoinValueUnformatted == null)
            {
                throw new ValueNotFound($"No value found for bitcoin in CoinMarketCap");
            }
            Price bitcoinPrice = priceFormatter.FormatPrice(bitcoinValueUnformatted);
            return new CollectResult(bitcoinPrice, DateTime.Now);
        }
    }
}
