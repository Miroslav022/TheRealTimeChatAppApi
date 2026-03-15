using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatApp.Application.Abstractions.Jwt;
using RealTimeChatApp.Domain.Entities;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealTimeChatApp.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string Generate(User user)
    {
        //var claims = new Claim[] { 
        //    new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //    new (JwtRegisteredClaimNames.Email, user.Email.ToString()),
        //    new (JwtRegisteredClaimNames.UniqueName, user.Username.ToString())
        //};

        //var signingCredentials = new SigningCredentials(
        //    new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
        //    SecurityAlgorithms.HmacSha256
        //    );

        //var token = new JwtSecurityToken(
        //    _jwtOptions.Issuer,
        //    _jwtOptions.Audience,
        //    claims,
        //    null,
        //    DateTime.UtcNow.AddHours(1),
        //    signingCredentials
        //    );

        string secretKey = _jwtOptions.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                    new Claim(JwtRegisteredClaimNames.Picture, user.ProfilePicture)
                ]),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
        //string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        //return tokenValue;
    }
}
