using RealTimeChatApp.Application.Abstractions.Command;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.RefreshToken;

public record RefreshTokenCommand(string refresh_token) : ICommand<string>;
