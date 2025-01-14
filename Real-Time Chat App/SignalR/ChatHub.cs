using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Real_Time_Chat_App.SignalR.DTOs;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.UseCases.Conversations.DTOs;
using RealTimeChatApp.Domain.Entities;

namespace Real_Time_Chat_App.SignalR
{
    //[Authorize]
    public sealed class ChatHub : Hub
    {
        private static Dictionary<string, string> _connectedUsers = new();
        private static Dictionary<string, IReadOnlyList<ConversationDto>> _userContacts = new();
        private readonly IChatRepository _chatRepository;
        private readonly IConversationRepository _conversationRepository;

        public ChatHub(IChatRepository chatRepository, IConversationRepository conversationRepository)
        {
            _chatRepository = chatRepository;
            _conversationRepository = conversationRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context?.UserIdentifier;
            if(!string.IsNullOrEmpty(userId))
            {
                _connectedUsers.Add(Context.ConnectionId, userId);

                var contacts = await _conversationRepository.GetAllUsersConversations(int.Parse(userId), CancellationToken.None);

                if(contacts != null)
                {
                    _userContacts.Add(userId, contacts);
                    foreach (var contact in contacts)
                    {
                        var contactConnectionId = _connectedUsers.FirstOrDefault(x => x.Value == contact.UserId.ToString()).Key;
                        if(contactConnectionId != null)
                        {
                            await Clients.Client(contactConnectionId).SendAsync("UserStatusChanged", userId, true);
                        }

                    }
                }

                await Clients.All.SendAsync("UserStatusChanged", userId);
            }
            var user = Context?.User?.Identity?.Name;
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context?.UserIdentifier;
            if(!string.IsNullOrEmpty(userId))
            {
                _connectedUsers.Remove(Context.ConnectionId);

                if(_userContacts.TryGetValue(userId, out var contacts))
                {
                    foreach (var contact in contacts)
                    {
                        var contactConnectionId = _connectedUsers.FirstOrDefault(x => x.Value == contact.UserId.ToString()).Key;
                        if(contactConnectionId != null)
                        {
                            await Clients.Client(contactConnectionId).SendAsync("UserStatusChanged", userId, false);
                        }
                await Clients.All.SendAsync("UserStatusChanged", userId);
            }
            await base.OnDisconnectedAsync(exception);
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

        public async Task TypingNotification(string roomId, string userName)
        {
            await Clients.OthersInGroup(roomId).SendAsync("ReceiveTypingNotification", userName);
        }

        public async Task JoinGroup(string groupName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
        }
    }
}
