using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.EditUser;

public sealed class EditUserCommand : ICommand
{
    public int id { get; set; }
    public string username { get; init; }
    public string email { get; set; }
    public string password { get; init; }

}