using RealTimeChatApp.Application.Abstractions.Command;
using System.Windows.Input;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Login;

public record LoginCommand(string email, string password) : ICommand<string>;
