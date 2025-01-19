using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Domain.Formatters
{
    public interface IPriceFormatter
    {
        public Price FormatPrice(string price);
    }
}
