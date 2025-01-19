using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCheck.Domain.Exceptions;

namespace CryptoCheck.Domain.Formatters
{
    public class CoinMarketCapPriceFormatter : IPriceFormatter
    {
        private Currency getCurrency(string currencyIndicator)
        {
            if(currencyIndicator == "$")
            {
                return Currency.DOLLARS;
            }
            throw new CurrencyNotFound($"It was not possible to determine which currency the indicator {currencyIndicator} represents");
        }
        private decimal transformStringValueToNumber(string value)
        {
            return Convert.ToDecimal(value);
        }
        public Price FormatPrice(string price)
        {
            char currencyIndicator = price[0];
            Currency currency = getCurrency(currencyIndicator.ToString());
            string valueString = price.Substring(1);
            decimal value = transformStringValueToNumber(valueString);
            return new Price(value, currency);
        }
    }
}
