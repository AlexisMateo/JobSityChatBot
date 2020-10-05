using System.Threading.Tasks;
using JobSity.ChatApp.Core.Entities.Bot;
using System.Collections.Generic;

namespace JobSity.ChatApp.Core.Interfaces.Bot
{
    public interface IBrokerService
    {
        Task<IEnumerable<Stock>> GetStockQuote(string stockCode);
    }
}