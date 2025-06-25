using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatApp.Application.Abstractions.Jwt;
using RealTimeChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Authentication
{
    public class RefreshJwtProvider : IJwtRefreshProvider
    {
        private readonly JwtOptions _jwtOptions;
        public RefreshJwtProvider(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string Generate(User user)
        {
            var claims = new Claim[]
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims, 
                expires: DateTime.UtcNow.AddDays(15),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
