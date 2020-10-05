using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Core.Entities.Bot;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.IO;
using CsvHelper;
using System.Linq;
using System;

namespace JobSity.ChatApp.Infrastructure.Services.Bot
{
    public class BrokerService : IBrokerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BrokerService> _logger;

        public BrokerService(HttpClient httpClient, ILogger<BrokerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Stock>> GetStockQuote(string url)
        {
            IEnumerable<Stock> stocks = new List<Stock>();

            try{
            
                var response =  await _httpClient.GetStringAsync(url);

                using(var content = new StringReader(response))
                {
                    var csv = new CsvReader(content, CultureInfo.InvariantCulture);
                    
                    stocks = csv.GetRecords<Stock>().ToList();
                    
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex.ToString());
            }
            
            return stocks;

        }
    }
}