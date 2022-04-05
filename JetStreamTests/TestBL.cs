using Jetstream.BLL;
using Jetstream.Data;
using Jetstream.Data.Fixer;
using Jetstream.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JetStreamTests
{
    [TestClass]
    public class TestBL
    {
        IExchangeRate _exchanger = new FixerIO("ad8b85c04432af2092d81b9aad3cf9de", "http://data.fixer.io/api/");

        [DataTestMethod]
        [DataRow(2020, 8, 5, 123.45, CurrencyCode.USD, 146.57, 14.66, 161.23, 1.187247)]
        [DataRow(2019, 7, 12, 1000.0, CurrencyCode.EUR, 1000, 90, 1090, 1.0)]
        [DataRow(2020, 8, 19, 6543.21, CurrencyCode.CAD, 10239.07, 1126.30, 11365.37, 1.564839)]
        public void TestMethod(int year, int month, int day, double amountEUR, 
                                CurrencyCode toCurrency, double preTax, double tax, 
                                double total, double exchangeRate)
        {
            InvoiceBL bl = new InvoiceBL(_exchanger);

            Invoice invoice = bl.GenerateInvoice(new DateTime(year, month, day), amountEUR, toCurrency).Result;

            Assert.IsNotNull(invoice);
            Assert.AreEqual(invoice.ExchangeRate.ToString(), exchangeRate.ToString());
            Assert.AreEqual(invoice.PreTaxAmount.ToString(), preTax.ToString());
            Assert.AreEqual(invoice.GrandTotal.ToString(), total.ToString());
            Assert.AreEqual(invoice.TaxAmount.ToString(), tax.ToString()); ;
        }
    }
}
