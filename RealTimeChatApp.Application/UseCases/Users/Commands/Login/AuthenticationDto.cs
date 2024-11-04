namespace RealTimeChatApp.Application.UseCases.Users.Commands.Login;

public class AuthenticationDto
{
    public string Access_token { get; set; }
    public string Refresh_token { get; set; }
}
