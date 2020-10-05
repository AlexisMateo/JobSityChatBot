using System.Threading.Tasks;
using JobSity.ChatApp.Bot.Models;
using System.Collections.Generic;

namespace JobSity.ChatApp.Bot.Interfaces
{
    public interface IBrokerService
    {
        Task<IEnumerable<Stock>> GetStockQuote(string stockCode);
    }
}