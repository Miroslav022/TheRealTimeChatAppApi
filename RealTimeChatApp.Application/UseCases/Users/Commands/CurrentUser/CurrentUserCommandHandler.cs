﻿using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Abstractions.Services;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.CurrentUser;

public class CurrentUserCommandHandler : ICommandHandler<CurrentUserCommand, CurrentUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public CurrentUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<CurrentUserDto>> Handle(CurrentUserCommand request, CancellationToken cancellationToken)
    {
        string access_token = request.Token;
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(access_token);
        var UserId = principal.FindFirst(JwtRegisteredClaimNames.Sub);
        User currentUser = await _userRepository.GetUserByIdAsync(UserId.Value);
        if (currentUser is not null)
        {
            return new CurrentUserDto
            {
                Email = currentUser.Email,
                Username = currentUser.Username,
                PhoneNumber = currentUser.PhoneNumber,
                ProfilePicture = currentUser.ProfilePicture,
                IsBlocked = currentUser.IsBlocked,
            };
        }
        return Result.Failure<CurrentUserDto>(new Error("Invalid_User_Id", "User with the provided id doesn't exist"));
    }
}