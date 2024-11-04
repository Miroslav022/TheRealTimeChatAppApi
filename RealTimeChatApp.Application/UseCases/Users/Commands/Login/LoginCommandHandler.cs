using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Jwt;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Services;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthenticationDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IJwtRefreshProvider _jwtRefreshProvider;
    private readonly AuthenticationService _authenticationService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, AuthenticationService authenticationService, IJwtRefreshProvider jwtRefreshProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _authenticationService = authenticationService;
        _jwtRefreshProvider = jwtRefreshProvider;
    }


    public async Task<Result<AuthenticationDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var authenticationResult = _authenticationService.Authenticate(request.email, request.password);
        if (authenticationResult.IsFailure) {
            //return Result.Failure<string>(authenticationResult.Error);
            return Result.Failure<AuthenticationDto>(authenticationResult.Error);
        }
        var user = authenticationResult.Value;
        string token = _jwtProvider.Generate(user);
        string refresh_token = _jwtRefreshProvider.Generate(user);
        return new AuthenticationDto { Access_token = token, Refresh_token = refresh_token};
    }
}
