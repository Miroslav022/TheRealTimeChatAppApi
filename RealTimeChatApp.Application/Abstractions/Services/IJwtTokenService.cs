using System.Security.Claims;

namespace RealTimeChatApp.Application.Abstractions.Services;

public interface IJwtTokenService
{
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
