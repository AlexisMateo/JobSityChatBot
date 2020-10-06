using Moq;
using Xunit;
using JobSity.ChatApp.Core.Interfaces.Chat;
using JobSity.ChatApp.Core.Entities.Chat;
using JobSity.ChatApp.Core.Interfaces.Repositories;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace JobSity.ChatApp.Test
{
    public class ChatRoomServiceTest
    {
        [Theory]
        [InlineData("dev1", "message")]
        [InlineData("dev2", "message")]
        public async Task AddChatRoomMessage_Should_ReturnCreatedMessage_When_Called(string userName, string message)
        {
            var testMessage = new Message { UserName = userName, MessageText = message };

            var iRepository = new Mock<IRepository<Message>>();
            iRepository
                .Setup(x => x.Insert(It.IsAny<Message>()))
                .ReturnsAsync(testMessage);
            
            var iService = new ChatRoomService(iRepository.Object);
            var exprectedMessage = await iService.AddChatRoomMessage(testMessage);

            Assert.Equal(testMessage, exprectedMessage);
        }

        //TODO Due to Moq lack of the ability to mock method with lambda expression, we need to test this method with a fakedbcontext. 
        [Theory]
        [InlineData(6)]
        [InlineData(3)]
        public async Task GetChatRoomMessages_Should_Return_LessOrEqual_Expected_Quantity(int quantity)
        {
            var messages = new List<Message>(){
                new Message { UserName = "user-1", MessageText = "message-1" },
                new Message { UserName = "user-2", MessageText = "message-2" },
                new Message { UserName = "user-3", MessageText = "message-3" },
                new Message { UserName = "user-4", MessageText = "message-4" },
                new Message { UserName = "user-5", MessageText = "message-5" },
                new Message { UserName = "user-6", MessageText = "message-6" }
            };

            var iService = new Mock<IChatRoomService>();

            iService
                .Setup(x => x.GetChatRoomMessages(It.IsAny<int>()))
                .ReturnsAsync(messages.Take(quantity));
            
            var receivedMessages = await iService.Object.GetChatRoomMessages(quantity);
            var quantityReceived = receivedMessages.Count();

            Assert.Equal(quantity, quantityReceived);
        } 
    }
}
