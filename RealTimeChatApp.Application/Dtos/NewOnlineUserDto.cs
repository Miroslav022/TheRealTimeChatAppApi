namespace RealTimeChatApp.Application.Dtos;

public sealed record NewOnlineUserDto(int UserId, string UserName, string profilePicture, bool isBlocked, int id)
{
}
