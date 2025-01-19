using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace CryptoCheck.Domain.PriceRetrievers
{
    public interface IPriceRetriever
    {
        public CollectResult Collect(IWebDriver webDriver);
    }
}
