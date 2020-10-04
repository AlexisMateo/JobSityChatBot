using System;

namespace JobSity.ChatApp.Core.Entities.Chat
{
    public class Message
    {
        public string MessageId { get; set; }
        public string UserName { get; set; }
        public string MessageText { get; set;}
        public DateTime SentDate { get; set; }
    }
}