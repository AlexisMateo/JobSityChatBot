using System.Collections.Generic;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Entities.Chat;

namespace JobSity.ChatApp.Core.Interfaces.Chat
{
    public interface IChatRoomService
    {
         Task<Message> AddChatRoomMessage(Message message);
         Task<IEnumerable<Message>> GetChatRoomMessages(int lastedQuantity);
    }
}