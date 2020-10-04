using JobSity.ChatApp.Core.Entities.Chat;
using JobSity.ChatApp.Core.Interfaces.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace JobSity.ChatApp.Infrastructure.Services
{
    [Authorize]
    public class ChatHubService : Hub
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatHubService(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            await _chatRoomService.AddChatRoomMessage(
                new Message {
                    UserName = user,
                    MessageText = message,
                    SentDate = DateTime.Now,
                });
        }

        public async override Task OnConnectedAsync()
        {
            await SendTheLastedMessageToTheUser();
        }

        private async Task SendTheLastedMessageToTheUser()
        {
            var messages = await _chatRoomService.GetChatRoomMessages(50);

            foreach(var message in messages)
            {

                    await Clients.User(Context.ConnectionId).SendAsync(
                        "ReceiveMessage", message.UserName, message.MessageText
                    );

            }
        }

    }
}