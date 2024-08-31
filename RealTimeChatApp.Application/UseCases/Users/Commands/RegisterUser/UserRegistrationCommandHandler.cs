using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Application.Command;
using RealTimeChatApp.DataAccess;
using RealTimeChatApp.Domain;
using System.Net.Http.Headers;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.RegisterUser;

public class UserRegistrationCommandHandler : ICommandHandler<UserRegistrationCommand>
{
    private readonly AspContext _aspContext;

    public UserRegistrationCommandHandler(AspContext aspContext)
    {
        _aspContext = aspContext;
    }

    public async Task Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        
        var user = new User
        {
            Username = request.email,
            Email = request.email,
            PasswordHash = request.password,
            PasswordSalt = "salt",
            PhoneNumber = request.phoneNumber
        };

        _aspContext.Users.Add(user);
        await _aspContext.SaveChangesAsync();

    }
}
