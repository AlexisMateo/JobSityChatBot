using System.Collections.Generic;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Entities.Chat;
using JobSity.ChatApp.Core.Interfaces.Chat;
using JobSity.ChatApp.Core.Interfaces.Repositories;
using System.Linq;

namespace JobSity.ChatApp.Core.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IRepository<Message> _messageRepository;

        public ChatRoomService(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<Message> AddChatRoomMessage(Message message)
        {
            return await _messageRepository.Insert(message);
        }

        public async Task<IEnumerable<Message>> GetChatRoomMessages(int quantity)
        {
            var messages =  await _messageRepository.Get( 
                orderBy: message => message.OrderByDescending(p => p.SentDate),
                limit: quantity
            );
            
            return messages.OrderBy(p => p.SentDate);
        }
    }
}