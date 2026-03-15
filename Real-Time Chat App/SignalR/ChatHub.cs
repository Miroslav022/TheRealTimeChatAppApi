using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Real_Time_Chat_App.SignalR.DTOs;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Domain.Entities;
using System.Collections.Concurrent;

namespace Real_Time_Chat_App.SignalR;

[Authorize]
public sealed class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, List<string>> _userConnectionsByUserId = new ConcurrentDictionary<string, List<string>>();


    private readonly IChatRepository _chatRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;
    public ChatHub(IChatRepository chatRepository, IContactRepository contactRepository, IConversationRepository conversationRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _contactRepository = contactRepository;
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var userId = Context?.UserIdentifier;
            var userObj = await _userRepository.GetUserByIdAsync(userId);

            if (!string.IsNullOrEmpty(userId))
            {
                _userConnectionsByUserId.AddOrUpdate(
                    userId,
                    _ => new List<string> { Context.ConnectionId },
                    (_, list) => { list.Add(Context.ConnectionId); return list; });

                var contacts = await _contactRepository.getContacts(int.Parse(userId));
                var conversations = await _conversationRepository.getAllUserConversationsWithoutGroups(int.Parse(userId));
                var conversationsMap = conversations.ToLookup(
                    c => string.Join(",", c.Participants.Select(x => x.UserId).Distinct().OrderBy(userId => userId)),
                    c => c.Id
                    );

                var userContacts = new List<OnlineUsersDto>();

                if (contacts != null)
                {
                    foreach (var contact in contacts)
                    {
                        //Notify user contacts that he is online
                        var contactId = contact.ContactUserId.ToString();
                        if (contactId == userId) continue;

                        if (_userConnectionsByUserId.TryGetValue(contactId, out var contactConnectionsIds))
                        {
                            // use dbContecxt to fetch conversation ID HERE!!!
                            var list = string.Join(",", new List<int> { int.Parse(userId), int.Parse(contactId) }.OrderBy(id => id));
                            var conversationId = conversationsMap[list].FirstOrDefault();
                            var newUser = new OnlineUsersDto(
                                int.Parse(userId),
                                contact.User.Username,
                                contact.User.ProfilePicture,
                                conversationId,
                                await _userRepository.IsUserBlocked(int.Parse(contactId), int.Parse(userId))
                            );

                            await Clients.Clients(contactConnectionsIds).SendAsync("newOnlineUser", newUser);
                            var test = userObj.BlockedContacts.Any(x => x.BlockedUserId == contact.ContactUserId);
                            var onlineContacts = new OnlineUsersDto(
                                contact.ContactUserId,
                                contact.ContactUser.Username,
                                contact.ContactUser.ProfilePicture,
                                conversationId,
                                userObj.BlockedContacts.Any(x => x.BlockedUserId == contact.ContactUserId)
                                );

                            userContacts.Add(onlineContacts);
                        }
                    }

                    await Clients.Client(Context.ConnectionId).SendAsync("UserStatusChanged", userContacts);
                }
            }
            var user = Context?.User?.Identity?.Name;

            await base.OnConnectedAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context?.UserIdentifier;
        var connectionId = Context?.ConnectionId;
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(connectionId)) return;

        if (_userConnectionsByUserId.TryGetValue(userId, out var connectionList))
        {
            lock (connectionList)
            {
                connectionList.Remove(connectionId);
                if (connectionList.Count == 0)
                {
                    _userConnectionsByUserId.TryRemove(userId, out _);
                }
            }

            var contacts = await _contactRepository.getContacts(int.Parse(userId));

            if (contacts.Count == 0) return;

            foreach (var contact in contacts)
            {
                var contactId = contact.ContactUserId.ToString();
                if (_userConnectionsByUserId.TryGetValue(contactId, out var contactConnectionIds))
                {
                    await Clients.Clients(contactConnectionIds).SendAsync("UserWentOffline", userId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
    private string GenerateRoomId(int conversationId)
    {
        return conversationId.GetHashCode().ToString();
    }

    public async Task JoinPrivateChat(int conversationId)
    {
        var roomId = GenerateRoomId(conversationId);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        await Clients.Caller.SendAsync("JoinedRoom", roomId);
    }

    public async Task SendMessage(ChatMessageDto chatMessageDto)
    {

        var roomId = GenerateRoomId(chatMessageDto.conversationId);

        var message = new Message
        {
            ConversationId = chatMessageDto.conversationId,
            SenderId = chatMessageDto.senderId,
            MessageTypeId = chatMessageDto.messageTypeId,
            MessageContent = chatMessageDto.message,
            RepliedToMessageId = chatMessageDto.replyToMessageId
        };
        try
        {
            await _chatRepository.SaveMessageAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // implement validation and fallback
        await Clients.Users(chatMessageDto.participantIds)
        .SendAsync("ReceivePrivateMessage", Context.User.Identity.Name, chatMessageDto.message, $"{DateTime.Now.Hour}:{DateTime.Now.Minute}");

        //await Clients.Group(roomId).SendAsync("ReceivePrivateMessage", Context.User.Identity.Name, chatMessageDto.message, $"{DateTime.Now.Hour}:{DateTime.Now.Minute}");
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
