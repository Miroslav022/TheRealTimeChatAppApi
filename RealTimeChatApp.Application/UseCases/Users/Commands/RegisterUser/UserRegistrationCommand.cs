using RealTimeChatApp.Application.Command;


namespace RealTimeChatApp.Application.UseCases.Users.Commands.RegisterUser;

public sealed record UserRegistrationCommand (
    string phoneNumber,
    string username,
    string email,
    string password) : ICommand;

