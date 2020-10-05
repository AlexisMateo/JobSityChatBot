namespace JobSity.ChatApp.Core.Interfaces.Bot
{
    public interface IBrokerProducerService 
    {
        void SendMessage(string queueName, string stockCode);
    }
}