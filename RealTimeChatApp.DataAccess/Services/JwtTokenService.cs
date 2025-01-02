using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatApp.Application.Abstractions.Services;
using RealTimeChatApp.Domain.Shared;
using RealTimeChatApp.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeChatApp.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        if (token is null) return Result.Failure<ClaimsPrincipal>(Error.UnAuthorized("Token.Null", "There is no user token"));


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };


        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.MapInboundClaims = false;
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }
        catch(SecurityTokenException ex)
        {
            throw;
        }
    }
}
