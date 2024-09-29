﻿using Microsoft.AspNetCore.SignalR;

namespace Real_Time_Chat_App.SignalR
{
    public sealed class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }

        private string GenerateRoomId(string user1Id, string user2Id)
        {
            return string.Join("-", new List<string> { user1Id, user2Id }.OrderBy(id => id));
        }

        public async Task JoinPrivateChat(string user1Id, string user2Id)
        {
            var roomId = GenerateRoomId(user1Id, user2Id);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Caller.SendAsync("JoinedRoom", roomId);

        }

        public async Task SendMessage(string user1Id, string user2Id, string message)
        {
            var roomId = GenerateRoomId(user1Id, user2Id);
            await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public async Task JoinGroup(string groupName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
        }
    }
}
