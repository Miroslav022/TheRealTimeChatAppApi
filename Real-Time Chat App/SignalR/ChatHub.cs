using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Real_Time_Chat_App.SignalR.DTOs;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Domain.Entities;

namespace Real_Time_Chat_App.SignalR
{
    //[Authorize]
    public sealed class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;

        public ChatHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public override Task OnConnectedAsync()
        {
            var user = Context?.User?.Identity?.Name;
            
            return base.OnConnectedAsync();
        }

        private string GenerateRoomId(int user1Id, int user2Id)
        {
            return string.Join("-", new List<int> { user1Id, user2Id }.OrderBy(id => id));
        }

        public async Task JoinPrivateChat(int user1Id, int user2Id)
        {
            var roomId = GenerateRoomId(user1Id, user2Id);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Caller.SendAsync("JoinedRoom", roomId);
        }

        public async Task SendMessage(ChatMessageDto chatMessageDto)
        {
            var roomId = GenerateRoomId(chatMessageDto.senderId, chatMessageDto.receiverId);

            var message = new Message
            {
                ConversationId = chatMessageDto.conversationId,
                SenderId = chatMessageDto.senderId,
                MessageTypeId = chatMessageDto.messageTypeId,
                MessageContent = chatMessageDto.message,
                RepliedToMessageId = null,
            };
            try
            {

                await _chatRepository.SaveMessageAsync(message);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            await Clients.Group(roomId).SendAsync("ReceivePrivateMessage", Context.User.Identity.Name, chatMessageDto.message, $"{DateTime.Now.Hour}:{DateTime.Now.Minute}");
        }

        public async Task JoinGroup(string groupName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
        }
    }
}
