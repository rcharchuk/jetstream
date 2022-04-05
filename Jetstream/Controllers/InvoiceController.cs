using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Jetstream.BLL;
using Jetstream.Data;
using Jetstream.Data.Fixer;
using Jetstream.Models;

namespace Jetstream.Controllers
{
    public class InvoiceController : ApiController
    {
        //Initialize out InvoiceBL with a Fixer source
        private IInvoiceBL _invoiceBL = new InvoiceBL();

        [HttpGet]
        [Route("api/Invoice/{invoiceDate:datetime}/{amountEUR:double}/{currencyCode:length(3)}")]
        public async Task<Invoice> Get(DateTime invoiceDate, double amountEUR, string currencyCode)
        {
            CurrencyCode toCurrency;

            //Make sure that the currency code is valid.
            if (!Enum.TryParse<CurrencyCode>(currencyCode, out toCurrency))
            {
                //If not, throw an exception
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Invalid currencyCode: {currencyCode}"),
                    ReasonPhrase = "currencyCode Not Found"
                });
            }

            if (amountEUR < 0)
            {
                //If not, throw an exception
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"amountEUR must be >= 0: {amountEUR}"),
                    ReasonPhrase = "Invalid amountEUR"
                });
            }

            if (amountEUR == 0)
            {
                return new Invoice();
            }

            Invoice invoice = null;

            try
            {
                //Invoke the BL to create a new invoice
                invoice = await _invoiceBL.GenerateInvoice(invoiceDate, amountEUR, toCurrency);
            }
            catch (Exception e)
            {
                //Catch and rethrow any exceptions
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(e.Message),
                    ReasonPhrase = "Exception Thrown"
                });
            }
           
            return invoice;
        }
    }
}