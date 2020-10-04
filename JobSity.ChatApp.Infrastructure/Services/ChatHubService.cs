using JobSity.ChatApp.Core.Entities.Chat;
using JobSity.ChatApp.Core.Interfaces.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JobSity.ChatApp.Infrastructure.Services
{
    [Authorize]
    public class ChatHubService : Hub
    {
         private IServiceProvider _serviceProvider;
        public ChatHubService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            await AddMessageToStorage(user, message);
           
        }

        public async override Task OnConnectedAsync()
        {
            await SendTheLastedMessageToTheUser();
        }

        private async Task AddMessageToStorage(string user, string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var chatRoomService = scope.ServiceProvider.GetRequiredService<IChatRoomService>();
                
                await chatRoomService.AddChatRoomMessage(
                    new Message {
                        UserName = user,
                        MessageText = message,
                        SentDate = DateTime.Now,
                });
            }
        }

        private async Task SendTheLastedMessageToTheUser()
        {
             using (var scope = _serviceProvider.CreateScope())
            {
                var chatRoomService = scope.ServiceProvider.GetRequiredService<IChatRoomService>();
                var messages = await chatRoomService.GetChatRoomMessages(50);

                foreach(var message in messages)
                {

                        await Clients.User(Context.ConnectionId).SendAsync(
                            "ReceiveMessage", message.UserName, message.MessageText
                        );

                }
            }
        }

    }
}