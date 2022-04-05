using Jetstream.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Jetstream.Data.Fixer
{
    public class FixerIO : IExchangeRate
    {
        private string _apiKey;
        private string _baseURL;

        public FixerIO()
        {
            //Get our settings from the config
            _apiKey  = ConfigurationManager.AppSettings["FixerIOApiKey"];
            _baseURL = ConfigurationManager.AppSettings["FixerIOBaseURL"];
        }

        public FixerIO(string apikey, string baseURL)
        {
            _apiKey = apikey;
            _baseURL = baseURL;
        }

        //Our interface function
        public async Task<double> GetExchangeRate(CurrencyCode from, CurrencyCode to, DateTime date)
        {
            //Generate our fixer url
            var url = GenerateUrl(date);

            using (var client = new HttpClient())
            {
                //Make the api call
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                //Parse out the exchange rate from the return value
                return ParseRate(await response.Content.ReadAsStringAsync(), from, to);
            }
        }

        //Parses the exchange rate from the data returned by fixer
        private double ParseRate(string data, CurrencyCode from, CurrencyCode to)
        {
            var root = JObject.Parse(data);
            var success = root.Value<bool>("success");

            //Check the return value from fixer
            if (!success)
            { 
                var error = root.Value<JObject>("error");
                throw new Exception(error.Value<string>("info"));
            }

            //If the call succeeded extract our rates for each input currency...
            var rates = root.Value<JObject>("rates");
            var fromRate = rates.Value<double>(from.ToString());
            var toRate = rates.Value<double>(to.ToString());

            //...and return the rate 
            return toRate / fromRate;
        }

        private string GenerateUrl(DateTime date)
        {
           return $"{_baseURL}{date.ToString("yyyy-MM-dd")}?access_key={_apiKey}";
        }

    }
}