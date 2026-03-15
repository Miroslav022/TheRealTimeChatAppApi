namespace Real_Time_Chat_App.SignalR.DTOs;

public record OnlineUsersDto(int UserId, string displayName, string displayImage, int id, bool isBlocked)
{
}
