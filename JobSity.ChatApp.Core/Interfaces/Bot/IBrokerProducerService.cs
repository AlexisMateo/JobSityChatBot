using System;
using JobSity.ChatApp.Core.Events;

namespace JobSity.ChatApp.Core.Interfaces.Bot
{
    public interface IBrokerProducerService 
    {
        event EventHandler<StockEventArgs> QueueCallBack;
        void SendMessage(string queueName, string stockCode);
    }
}