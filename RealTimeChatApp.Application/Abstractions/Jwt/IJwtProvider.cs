using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Application.Abstractions.Jwt;

public interface IJwtProvider
{
    public string Generate(User user);
}
