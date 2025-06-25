namespace RealTimeChatApp.Application.UseCases.Contacts.DTOs;

public sealed record ContactsDto(int userId, string username, string profilePicture, string phoneNumber)
{
}
