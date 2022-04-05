using System;
using System.Threading.Tasks;
using Jetstream.Data;
using Jetstream.Models;

namespace Jetstream.BLL
{
    public interface IInvoiceBL
    {
        Task<Invoice> GenerateInvoice(DateTime date, double amountEUR, CurrencyCode toCurrency);
    }

    public class InvoiceBL: IInvoiceBL
    {
        private IExchangeRate _exchangeRate;

        public InvoiceBL()
        {
            _exchangeRate = ExchangeRateFactory.GetExchangeRateInstance();
        }
        
        public async Task<Invoice> GenerateInvoice(DateTime date, double amountEUR, CurrencyCode toCurrency)
        {
            double exchangeRate = 1.0;

            //No need to query the exchange rate if the currency isnt changing
            if (toCurrency != CurrencyCode.EUR)
            {
                exchangeRate = await _exchangeRate.GetExchangeRate(CurrencyCode.EUR, toCurrency, date);
            }

            //Calculate the values for our invoice...
            var preTaxAmount = Math.Round(amountEUR * exchangeRate, 2); ;
            var taxAmount = Math.Round(preTaxAmount * GetTaxRate(toCurrency), 2);
            var grandTotal = preTaxAmount + taxAmount;

            //...and return it
            return new Invoice()
            {
                ExchangeRate = exchangeRate,
                PreTaxAmount = preTaxAmount,
                GrandTotal = grandTotal,
                TaxAmount = taxAmount
            };
        }

        public double GetTaxRate(CurrencyCode currencyCode)
        {
            switch (currencyCode)
            {
                case CurrencyCode.USD:
                    return 0.10;
                case CurrencyCode.CAD:
                    return 0.11;
                case CurrencyCode.EUR:
                    return 0.09;
                default:
                    break;
            }

            throw new Exception("GetTaxRate: CurrencyCode not found");
        }

    }
}