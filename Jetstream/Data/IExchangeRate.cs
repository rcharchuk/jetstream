using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jetstream.Data
{
    public enum CurrencyCode
    {
        USD,
        CAD,
        EUR
    }

    //Interface for querying exchange rates
    public interface IExchangeRate
    {
        Task<double> GetExchangeRate(CurrencyCode from, CurrencyCode to, DateTime date);
    }
}
