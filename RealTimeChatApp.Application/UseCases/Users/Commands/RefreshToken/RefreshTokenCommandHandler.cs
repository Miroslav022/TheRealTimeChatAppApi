﻿using MediatR;
using RealTimeChatApp.Application.Abstractions.Command;
using RealTimeChatApp.Application.Abstractions.Jwt;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Application.Abstractions.Services;
using RealTimeChatApp.Domain.Entities;
using RealTimeChatApp.Domain.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace RealTimeChatApp.Application.UseCases.Users.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, string>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(IJwtProvider jwtProvider, IJwtTokenService jwtTokenService, IUserRepository userRepository)
    {
        _jwtProvider = jwtProvider;
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (request.refresh_token == null) 
        {
            return Result.Failure<string>(Error.UnAuthorized("Obtain token failed", "You don't have refresh token"));
        }
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.refresh_token);

        if (principal == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var userIdClaim = principal.Value.FindFirst(JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Invalid user");
        }

        var user = await _userRepository.GetUserByIdAsync(userIdClaim.Value);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var newAccessToken = _jwtProvider.Generate(user);

        return newAccessToken;
    }

}
