using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jetstream.Models
{
    //Invoice class definition
    public class Invoice
    {
        public double PreTaxAmount { get; set; }
        public double TaxAmount { get; set; }
        public double GrandTotal { get; set; }
        public double ExchangeRate { get; set; }

        public Invoice()
        {
            PreTaxAmount = 0;
            TaxAmount = 0;
            GrandTotal = 0;
            ExchangeRate = 0;
        }
    }
}