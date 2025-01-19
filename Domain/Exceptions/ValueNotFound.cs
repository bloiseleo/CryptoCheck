using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Domain.Exceptions
{
    public class ValueNotFound(string message): Exception(message)
    {
    }
}
