namespace RealTimeChatApp.Application.UseCases.Users.DTOs;

public sealed record UserDto(string Email, string UserName,int Id, string PhoneNumber, bool IsBlocked, string ProfilePicture);
