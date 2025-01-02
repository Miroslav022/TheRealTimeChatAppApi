using RealTimeChatApp.Domain.Shared;
using System.Security.Claims;

namespace RealTimeChatApp.Application.Abstractions.Services;

public interface IJwtTokenService
{
    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}
