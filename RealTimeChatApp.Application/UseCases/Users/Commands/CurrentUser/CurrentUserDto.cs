namespace RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;

public class CurrentUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsBlocked { get; set; }
    public string? ProfilePicture { get; set; }
}
