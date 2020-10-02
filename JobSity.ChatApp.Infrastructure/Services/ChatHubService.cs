using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JobSity.ChatApp.Infrastructure.Services
{
    public class ChatHubService : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ChatHub", message);
        }
    }
}