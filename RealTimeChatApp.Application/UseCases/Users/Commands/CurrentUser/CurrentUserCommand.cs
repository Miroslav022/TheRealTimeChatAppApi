using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;

public record CurrentUserCommand(string Token) : ICommand<CurrentUserDto>;
