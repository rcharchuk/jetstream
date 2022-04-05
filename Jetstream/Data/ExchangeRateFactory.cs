using Jetstream.Data.Fixer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jetstream.Data
{
    public class ExchangeRateFactory
    {
        public static IExchangeRate GetExchangeRateInstance()
        {
            return new FixerIO();
        }
    }
}