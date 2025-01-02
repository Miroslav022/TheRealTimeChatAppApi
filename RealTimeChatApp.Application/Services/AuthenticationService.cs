using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Services.Security;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;

namespace RealTimeChatApp.Application.Services;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Result<User> Authenticate(string email, string password)
    {
        var user = _userRepository.GetByEmail(email);
        if (user == null) {
            return Result.Failure<User>(Error.NotFound("Authentication Failed", "User not found"));
        }

        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash)) {
            return Result.Failure<User>(Error.Validation("Authentication Failed", "Invalid credentials"));
        }
        return Result.Success(user);
    }
}
