using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Real_Time_Chat_App.SignalR
{
    //[Authorize]
    public sealed class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var user = Context?.User?.Identity?.Name;
            
            return base.OnConnectedAsync();
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
            await Clients.Group(roomId).SendAsync("ReceivePrivateMessage", Context.User.Identity.Name, message, $"{DateTime.Now.Hour}:{DateTime.Now.Minute}");
        }

        public async Task JoinGroup(string groupName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} joined {groupName}");
        }
    }
}
