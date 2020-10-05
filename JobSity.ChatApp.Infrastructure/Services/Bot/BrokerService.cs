using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Core.Entities.Bot;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace JobSity.ChatApp.Infrastructure.Services.Bot
{
    public class BrokerService : IBrokerService
    {
        private readonly HttpClient _httpClient;
        public BrokerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Stock>> GetStockQuote(string url)
        {
            IEnumerable<Stock> stocks;

            var response =  await _httpClient.GetStringAsync(url);

            using(var textReader = new StreamReader(response))
            {
                var csv = new CsvReader(textReader, CultureInfo.InvariantCulture);
                
                stocks = csv.GetRecords<Stock>();
                
            }
            
            return stocks;

        }
    }
}