using JobSity.ChatApp.Core.Entities.Chat;
using JobSity.ChatApp.Core.Interfaces.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Entities.Bot;
using JobSity.ChatApp.Core.Interfaces.Bot;
using Microsoft.Extensions.Options;
using JobSity.ChatApp.Core.Utils;
using System.Text;
using JobSity.ChatApp.Core.Events;

namespace JobSity.ChatApp.Infrastructure.Services
{
    [Authorize]
    public class ChatHubService : Hub
    {
         private IServiceProvider _serviceProvider;
         private IBrokerProducerService _brokerProducerService;
         private readonly RabbitMQInfo _rabbitInfo;
         private readonly StockQueues _stockQueues;
         private const string botUser = "bot";

        public ChatHubService(IServiceProvider serviceProvider,
            IBrokerProducerService brokerProducerService,
            IOptions<RabbitMQInfo> rabbitInfo,
            IOptions<StockQueues> stockQueues
        )
        {
            _serviceProvider = serviceProvider;
            _brokerProducerService = brokerProducerService;
            _rabbitInfo = rabbitInfo.Value;
            _stockQueues = stockQueues.Value;

            _brokerProducerService.QueueCallBack += async (o, e) => {
                    await SendMessage(botUser, e.StockInfo);
                };
        }

        public async Task SendMessage(string user, string message)
        {
            var newMessage = new Message { UserName = user, MessageText = message, SentDate = DateTime.Now };

            var stockMessage = newMessage.MessageText.ExtractStock();
            
            if(!stockMessage.isStockMessage)
            {
                await Clients.All.SendAsync("ReceiveMessage", newMessage);
                
                if(user != botUser)
                {
                    await AddMessageToStorage(newMessage);
                }

            }
            else {

                _brokerProducerService.SendMessage(
                    queueName : _stockQueues.StockRequest,
                    stockCode: stockMessage.StockCode
                );

            }
           
        }

        public async override Task OnConnectedAsync()
        {
            await SendTheLastedMessageToTheUser();

            await base.OnConnectedAsync();
        }
 
        private async Task AddMessageToStorage(Message message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var chatRoomService = scope.ServiceProvider.GetRequiredService<IChatRoomService>();
                
                await chatRoomService.AddChatRoomMessage(message);
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

                        await Clients.Client(Context.ConnectionId).SendAsync(
                            "ReceiveMessage", message
                        );

                }
            }
        }

    }
}