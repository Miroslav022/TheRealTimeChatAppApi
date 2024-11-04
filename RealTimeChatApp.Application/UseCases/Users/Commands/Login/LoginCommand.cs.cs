using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Login;

public record LoginCommand(string email, string password) : ICommand<AuthenticationDto>;
